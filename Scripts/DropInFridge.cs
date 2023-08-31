using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/* class to drop ingredients in the fridge once stack is full */
public class DropInFridge : MonoBehaviour, IDropHandler
{
    private GameObject canvasGO;
    private GameObject promptGO;
    private int ingLocation = 0; //slot index for each dropped ingredient in the fridge
    
    void Start()
    {
        canvasGO = GameObject.Find("Canvas");
        promptGO = Resources.FindObjectsOfTypeAll<PromptDummy>()[0].gameObject;
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    public void OnDrop(PointerEventData eventData)
    {
        Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<StoreIngredients>().ingredientSlots;

        if (eventData.pointerDrag != null)
        {
            //get the ingredient dropped in fridge and the corresponding cell it belonged to
            GameObject droppedIngredient = eventData.pointerDrag.gameObject;
            GameObject droppedCell = hm[droppedIngredient];
            CanvasGroup ingCanvasGroup = droppedIngredient.GetComponent<CanvasGroup>();

            SCellAssigner sScript = droppedCell.GetComponent<SCellAssigner>(); //get the SCell identifier of the cell where the element was dropped

            if (!sScript.isTop) //if the corresponding cell is not the top
            {
                //snap the dropped ingredient back to its original cell
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = droppedIngredient.GetComponent<StackPopDrag>().originalPosition;

                //display prompt text
                DisplayPrompt("Can only pop from top of stack.");
            }
            else //if the corresponding cell is the top
            {
                //restore ingredient's original opacity and block raycasts so that it does not interact with any other UI elements
                ingCanvasGroup.alpha = 1f;
                ingCanvasGroup.blocksRaycasts = true;

                droppedIngredient.transform.position = transform.GetChild(ingLocation).position; //snap the dropped ingredient to its slot in the fridge

                //update top pointer
                sScript.isTop = false;
                int curr_cell_num = int.Parse(droppedCell.name);
                if (curr_cell_num != 1)
                { 
                    GameObject nextCell = GameObject.Find((curr_cell_num - 1).ToString());
                    nextCell.GetComponent<SCellAssigner>().isTop = true;
                    Vector2 topPos = GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition;
                    GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition = new Vector2(topPos.x, topPos.y - 85f);
                }

                //change tag of dropped ingredient to 'Stored'
                droppedIngredient.tag = "Stored";

                Destroy(droppedIngredient.GetComponent<StackPopDrag>()); //remove the 'StackPopDrag' logic from the dropped ingredient
                ingLocation++; //increase the slot index

                //after drop, put the note UI on top of all UI elements
                canvasGO.GetComponent<StoreIngredients>().note.transform.SetAsLastSibling();

                //once all ingredients are dropped in fridge enable finish button
                if (ingLocation == 10)
                {
                    canvasGO.GetComponent<StoreIngredients>().finish.gameObject.SetActive(true);
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
