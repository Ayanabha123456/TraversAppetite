using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/* class to drop ingredients in the pan once the queue is full*/
public class DropInPan : MonoBehaviour, IDropHandler
{
    private GameObject canvasGO;
    private GameObject promptGO;
    private int ingLocation = 0; //slot index for each ingredient dropped in the pan
    
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
        Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<SpawnIngredient>().ingredientSlots;

        if (eventData.pointerDrag != null)
        {
            //get the ingredient dropped in pan and the corresponding cell it belonged to
            GameObject droppedIngredient = eventData.pointerDrag.gameObject;
            GameObject droppedCell = hm[droppedIngredient];
            CanvasGroup ingCanvasGroup = droppedIngredient.GetComponent<CanvasGroup>();

            QCellAssigner qScript = droppedCell.GetComponent<QCellAssigner>(); //get the QCell identifier of the cell where the element was dropped

            if (!qScript.isFront) //if the corresponding cell is not the front
            {
                //snap the dropped ingredient back to its original cell
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = droppedIngredient.GetComponent<DequeueDrag>().originalPosition;

                //display prompt text
                DisplayPrompt("Can only dequeue from front of queue.");
            }
            else //if the corresponding cell is the front
            {
                //restore ingredient's original opacity and block raycasts so that it does not interact with any other UI elements
                ingCanvasGroup.alpha = 1f;
                ingCanvasGroup.blocksRaycasts = true;

                droppedCell.GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().originalColor; //restore original color of the cell

                droppedIngredient.transform.position = transform.GetChild(ingLocation).position; //snap the dropped ingredient to its slot in the pan

                //update front pointer
                qScript.isFront = false;
                int curr_cell_num = int.Parse(droppedCell.name);
                if (curr_cell_num != 1)
                {
                    GameObject nextCell = GameObject.Find((curr_cell_num - 1).ToString());
                    nextCell.GetComponent<QCellAssigner>().isFront = true;
                    Vector2 frontPos = GameObject.Find("front").GetComponent<RectTransform>().anchoredPosition;
                    GameObject.Find("front").GetComponent<RectTransform>().anchoredPosition = new Vector2(frontPos.x - 85f, frontPos.y);
                }

                //change tag of dropped ingredient to 'Cooked'
                droppedIngredient.tag = "Cooked";

                Destroy(droppedIngredient.GetComponent<DequeueDrag>()); //remove the 'DequeueDrag' logic from the dropped ingredient
                ingLocation++; //increase the slot index

                //after drop, put the note UI on top of all UI elements
                canvasGO.GetComponent<SpawnIngredient>().note.transform.SetAsLastSibling();

                //once all ingredients are dropped in pan enable finish button
                if (ingLocation == 10)
                {
                    canvasGO.GetComponent<SpawnIngredient>().finish.gameObject.SetActive(true);
                }
            }
        }
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for the specified display time
        yield return new WaitForSeconds(5f);

        promptGO.SetActive(false);
    }
}
