using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/* class to drag the ingredients once the queue is full and drop it in the pan */
public class DequeueDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private CanvasGroup ingCanvasGroup; //canvas group of the ingredient
    public Vector2 originalPosition; //original position of the ingredient in the queue
    private GameObject canvasGO;
    private GameObject promptGO;

    void Start()
    {
        ingCanvasGroup = GetComponent<CanvasGroup>();
        ingCanvasGroup.alpha = 1f;
        ingCanvasGroup.blocksRaycasts = true;

        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        canvasGO = GameObject.Find("Canvas");

        promptGO = Resources.FindObjectsOfTypeAll<PromptDummy>()[0].gameObject;
    }
    //on start of drag, reduce opacity of ingredient to visually indicate the drag event
    //also don't block the raycasts so that the pan can detect the dragged ingredient over it and receive it for the drop event
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

        Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<SpawnIngredient>().ingredientSlots;
        GameObject droppedCell = hm[gameObject]; //get queue cell in which the ingredient is dropped

        //if at the end of a drag, the ingredient is not in the pan, i.e. doesn't have the 'Cooked' tag
        if (gameObject.tag != "Cooked")
        {
            //snap it back to its original position in the queue cell
            gameObject.GetComponent<RectTransform>().anchoredPosition = droppedCell.GetComponent<RectTransform>().anchoredPosition;
        }
        else
        {
            droppedCell.GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().originalColor; //else restore the original color of the cell
        }
    }
}
