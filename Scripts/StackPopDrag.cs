using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/* class to drag the ingredients once the stack is full and drop it in the fridge */
public class StackPopDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private CanvasGroup ingCanvasGroup; //canvas group of the ingredient
    public Vector2 originalPosition; //original position of the ingredient in the stack
    private GameObject canvasGO;

    void Start()
    {
        ingCanvasGroup = GetComponent<CanvasGroup>();
        ingCanvasGroup.alpha = 1f;
        ingCanvasGroup.blocksRaycasts = true;

        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        canvasGO = GameObject.Find("Canvas");
    }
    //on start of drag, reduce opacity of ingredient to visually indicate the drag event
    //also don't block the raycasts so that the fridge can detect the dragged ingredient over it and receive it for the drop event
    public void OnBeginDrag(PointerEventData eventData)
    {
        ingCanvasGroup.alpha = .7f;
        ingCanvasGroup.blocksRaycasts = false;
    }
    //during drag, move the sprite in accordance with the mouse position
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    //triggers on end of drag
    public void OnEndDrag(PointerEventData eventData)
    {
        //at end of drag, you can restore original CanvasGroup values
        ingCanvasGroup.alpha = 1f;
        ingCanvasGroup.blocksRaycasts = true;

        Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<StoreIngredients>().ingredientSlots;
        GameObject droppedCell = hm[gameObject]; //get stack cell in which the ingredient is dropped

        //if at the end of a drag, the ingredient is not in the fridge, i.e. doesn't have the 'Stored' tag
        if (gameObject.tag != "Stored")
        {
            //snap it back to its original position in the stack cell
            gameObject.GetComponent<RectTransform>().anchoredPosition = droppedCell.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
