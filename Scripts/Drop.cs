using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/* class to drop ingredients in the queue cells for FIFO & BFS - it is attached to the queue cells in the CookingScene */
public class Drop : MonoBehaviour, IDropHandler
{
    public Image cellImage; //'Image' component of the queue cell - useful for changing its color
    public Color originalColor; //stores original color of the queue cells
    private GameObject droppedIngredient; //the dropped ingredient
    private Vector2 originalPosition; //original position of ingredient before drop
    private GameObject canvasGO;
    private GameObject promptGO;
    private string questType; //indicates the algorithm - FIFO or BFS

    void Start() 
    {
        cellImage = GetComponent<Image>();
        originalColor = cellImage.color;
        canvasGO = GameObject.Find("Canvas");
        questType = canvasGO.GetComponent<SpawnIngredient>().questType;
        promptGO = GameObject.Find("Prompt");
    }

    /* function to return an identifier for the dropped ingredient to check if it was dropped in the correct queue cell */
    string GetIngredientIdentifier(GameObject droppedIngredient)
    {
        if(questType == "FIFO") //for FIFO algorithm return the name of the dropped ingredient
            return droppedIngredient.name;
        else //for BFS algorithm return the level of the dropped ingredient in BFS
        {
            Dictionary<string, int> level = canvasGO.GetComponent<SpawnIngredient>().bfsResult;
            return level[droppedIngredient.name].ToString();
        }
    }

    /* function to highlight the next level of ingredients (specifically their ingredient slots) in the graph for BFS*/
    void HighlightNextLevel(int level)
    {
        ItemSlotAssigner[] gameObjs = GameObject.FindObjectsOfType<ItemSlotAssigner>();
        
        foreach(ItemSlotAssigner obj in gameObjs)
        {
            if(obj.cellID == (level - 1).ToString() ) //restore the original color of the previous level
            {
                obj.gameObject.GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor;
            }
            if(obj.cellID == (level).ToString()) //highlight the next level
            {
                obj.gameObject.GetComponent<Image>().color = Color.yellow;
            }
        }
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    public void OnDrop(PointerEventData eventData)
    {
        QCellAssigner qScript = gameObject.GetComponent<QCellAssigner>(); //get the QCell identifier of the cell where an element is dropped

        if(eventData.pointerDrag != null) //if an ingredient is dropped
        {
            droppedIngredient = eventData.pointerDrag.gameObject; //get the dropped ingredient
            droppedIngredient.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().gameObject.SetActive(false); //disable the hovering text
            
            //disable the revert button for the next step
            canvasGO.GetComponent<SpawnIngredient>().revert.gameObject.SetActive(false);

            if (!qScript.isRear) //if the cell is not the rear of the queue
            {
                //snap back to original position of the dropped ingredient
                originalPosition = droppedIngredient.GetComponent<DragAndDrop>().originalPosition;
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);

                //display prompt text
                DisplayPrompt("Can only enqueue at rear of queue.");
            }
            else //if the cell is the rear of the queue
            {
                //increase capacity of queue by 1
                canvasGO.GetComponent<SpawnIngredient>().capacity++;

                //For BFS algorithm, highlight the next level of neighbors (if current level is full)
                if (questType == "BFS")
                {
                    Dictionary<string, int> levelIngCount = canvasGO.GetComponent<SpawnIngredient>().levelIngCount; //stores no. of ingredients at each level of BFS
                    levelIngCount[canvasGO.GetComponent<SpawnIngredient>().currLevel.ToString()]--; //decrease count of current level ingredients by 1
                    canvasGO.GetComponent<SpawnIngredient>().prevLevel = canvasGO.GetComponent<SpawnIngredient>().currLevel; //set previous level = current level

                    if (levelIngCount[canvasGO.GetComponent<SpawnIngredient>().currLevel.ToString()] == 0) //if the no. of ingredients at current level drops to zero
                    {
                        canvasGO.GetComponent<SpawnIngredient>().currLevel++; //move on to next level of BFS
                        HighlightNextLevel(canvasGO.GetComponent<SpawnIngredient>().currLevel); //highlight the ingredients at next level of BFS
                    }
                }

                //snap the dropped ingredient into the queue cell
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                //if the dropped ingredient matches the ingredient the cell was expecting
                if (GetIngredientIdentifier(droppedIngredient) == qScript.cellID)
                    cellImage.color = Color.green; //change cell color to green
                else
                {
                    cellImage.color = Color.red; //else change to red

                    //set the correct prompt for the algorithm once a wrong ingredient is dropped
                    if (questType == "BFS")
                    {
                        if (gameObject.name != "10")
                            DisplayPrompt("Level of dropped ingredient is not the same as current level of neighbors.");
                        else
                            DisplayPrompt("Dropped ingredient is not the source/starting ingredient.");
                    }
                    else
                    {
                        DisplayPrompt("Dropped ingredient is not in the correct First-In-First-Out order.");
                    }
                }

                //modify the (dropped game object, slot) in the dictionary
                Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<SpawnIngredient>().ingredientSlots;
                hm[droppedIngredient] = gameObject;

                //change the Tag of dropped ingredient to 'Processed'
                droppedIngredient.tag = "Processed";

                //update the rear pointer
                qScript.isRear = false;
                int curr_cell_num = int.Parse(gameObject.name);
                if(curr_cell_num != 1)
                {
                    GameObject nextCell = GameObject.Find((curr_cell_num - 1).ToString());
                    nextCell.GetComponent<QCellAssigner>().isRear = true;
                    Vector2 rearPos = GameObject.Find("rear").GetComponent<RectTransform>().anchoredPosition;
                    GameObject.Find("rear").GetComponent<RectTransform>().anchoredPosition = new Vector2(rearPos.x - 85f, rearPos.y);
                }

                //after drop, put the note UI on top of all UI elements
                canvasGO.GetComponent<SpawnIngredient>().note.transform.SetAsLastSibling();

                //remove the 'DragAndDrop' script if correct ingredient is dropped
                if (GetIngredientIdentifier(droppedIngredient) == qScript.cellID)
                    Destroy(droppedIngredient.GetComponent<DragAndDrop>());
                else //else remove the 'DragAndDrop' script and enable the 'Revert' button
                {
                    Destroy(droppedIngredient.GetComponent<DragAndDrop>());
                    canvasGO.GetComponent<SpawnIngredient>().revert.gameObject.SetActive(true);
                }
                        
            }
            
        }
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        promptGO.SetActive(false);
    }
}
