using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/* a class to drag the ingredients around and drop it in the stack cells */
public class StackDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup ingCanvasGroup; // canvas group of the ingredient
    public Vector2 originalPosition; // original position of the ingredient
    private GameObject canvasGO;
    private GameObject ingredientText;
    private TextMeshProUGUI text;

    void Start()
    {
        ingCanvasGroup = GetComponent<CanvasGroup>();
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
    //also don't block the raycasts so that the stack cell can detect the dragged ingredient over it and receive it for the drop event
    public void OnBeginDrag(PointerEventData eventData)
    {
        ingCanvasGroup.alpha = .7f;
        ingCanvasGroup.blocksRaycasts = false;
    }
    //triggers on end of drag
    public void OnEndDrag(PointerEventData eventData)
    {
        //at end of drag, you can restore original CanvasGroup values
        ingCanvasGroup.alpha = 1f;
        ingCanvasGroup.blocksRaycasts = true;

        //if at the end of a drag, the ingredient does not have the tag 'Processed' i.e. not in a stack cell
        if (gameObject.tag != "Processed")
        {
            //snap it back to its original position in the Item Slot
            gameObject.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("ItemSlot").GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
