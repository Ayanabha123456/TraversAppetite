using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/* class to drag the ingredients around and drop it in the cells */
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup ingCanvasGroup; // canvas group of the ingredient
    public Vector2 originalPosition; // original position of the ingredient
    private GameObject canvasGO;
    private GameObject ingredientText;
    private TextMeshProUGUI text;


    void Start()
    {
        ingCanvasGroup = GetComponent<CanvasGroup>();
        originalPosition = GetComponent<RectTransform>().anchoredPosition;

        canvasGO = GameObject.Find("Canvas");
    
        ingredientText = transform.GetChild(0).gameObject;
        text = ingredientText.GetComponent<TextMeshProUGUI>();
    }
    //display name of ingredient on pointer entry
    public void OnPointerEnter(PointerEventData eventData)
    {
        ingredientText.SetActive(true);
        text.gameObject.SetActive(true);
    }
    //on pointer exit disable name of ingredient
    public void OnPointerExit(PointerEventData eventData)
    {
        ingredientText.SetActive(false);
        text.gameObject.SetActive(false);
    }
    //during drag, display name of ingredient and move the sprite in accordance with the mouse position
    public void OnDrag(PointerEventData eventData)
    {
        ingredientText.SetActive(true);
        text.gameObject.SetActive(true);
        transform.position = Input.mousePosition;
    }
    //on start of drag, reduce opacity of ingredient to visually indicate the drag event
    //also don't block the raycasts so that the cell can detect the dragged ingredient over it and receive it for the drop event
    public void OnBeginDrag(PointerEventData eventData)
    {
        ingCanvasGroup.alpha = .7f;
        ingCanvasGroup.blocksRaycasts = false;

        gameObject.transform.SetParent(GameObject.Find("Canvas").transform); //brings the ingredient out of its slot
    }
    //triggers on end of drag
    public void OnEndDrag(PointerEventData eventData)
    {
        //at end of drag, you can restore original CanvasGroup values
        ingCanvasGroup.alpha = 1f;
        ingCanvasGroup.blocksRaycasts = true;

        //if at the end of a drag, the ingredient doesn't have the 'Processed' tag i.e. not in a cell, 
        if (gameObject.tag != "Processed")
        {
            //snap it back to its original position i.e in its slot
            gameObject.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("_"+gameObject.name).GetComponent<RectTransform>().anchoredPosition;
            gameObject.transform.SetParent(GameObject.Find("_" + gameObject.name).transform); //set the slot as its parent
        }
    }
}
