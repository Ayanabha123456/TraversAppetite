using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/* class to drop ingredients in the stack cells for LIFO */
public class StackDrop : MonoBehaviour, IDropHandler
{
    private GameObject droppedIngredient; //the dropped ingredient
    private GameObject canvasGO;
    private GameObject promptGO;

    void Start()
    {
        canvasGO = GameObject.Find("Canvas");
        promptGO = GameObject.Find("Prompt");
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    public void OnDrop(PointerEventData eventData)
    {
        SCellAssigner sScript = gameObject.GetComponent<SCellAssigner>(); //get the SCell identifier of the cell where an element is dropped

        if (eventData.pointerDrag != null) //if an ingredient is dropped
        {
            droppedIngredient = eventData.pointerDrag.gameObject; //get the dropped ingredient
            droppedIngredient.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().gameObject.SetActive(false); //disable the hovering text

            if (!sScript.isTop) //if the cell is not the top of the stack
            {
                //snap the ingredient back to its original position
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("ItemSlot").GetComponent<RectTransform>().anchoredPosition;

                //display prompt text
                DisplayPrompt("Can only push at top of stack.");
            }
            else //if the cell is the top of the stack
            {
                //increase stack capacity by 1
                canvasGO.GetComponent<StoreIngredients>().capacity++;

                //snap the dropped ingredient into the stack cell
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
             
                //modify the (dropped game object, slot) in the dictionary
                Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<StoreIngredients>().ingredientSlots;
                hm[droppedIngredient] = gameObject;

                //change the Tag of ingredient to 'Processed' when placed in a stack cell
                droppedIngredient.tag = "Processed";

                //update the top pointer
                sScript.isTop = false;
                int curr_cell_num = int.Parse(gameObject.name);
                GameObject nextCell = GameObject.Find((curr_cell_num + 1).ToString());
                nextCell.GetComponent<SCellAssigner>().isTop = true;
                Vector2 topPos = GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition;
                GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition = new Vector2(topPos.x , topPos.y + 85f);

                //remove the 'StackDrag' script from the dropped ingredient
                Destroy(droppedIngredient.GetComponent<StackDrag>());

                //generate next ingredient in the stream
                canvasGO.GetComponent<StoreIngredients>().GenerateStreamIngredient();

                //after drop, put the note UI on top of all UI elements
                canvasGO.GetComponent<StoreIngredients>().note.transform.SetAsLastSibling();
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
