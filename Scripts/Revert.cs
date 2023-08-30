using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* a class to revert to the previous game state if the player drops a wrong ingredient in the queue/stack */
public class Revert : MonoBehaviour
{
    private GameObject canvasGO;
    private string questType; //the algorithm being dealt with
    
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        canvasGO = GameObject.Find("Canvas");
        questType = canvasGO.GetComponent<SpawnIngredient>().questType;
    }

    public void OnButtonClick()
    {
        SpawnIngredient canvasScript = canvasGO.GetComponent<SpawnIngredient>();
        int num_ing = canvasScript.numOfIngredients; //the total no. of ingredients
        int capacity = canvasScript.capacity; //curren capacity of the stack/queue
        Dictionary<GameObject,GameObject> ingredientSlots = canvasScript.ingredientSlots; //map that shows which ingredient is dropped in which cell
        Dictionary<GameObject,Vector2> positions = canvasScript.positions;

        GameObject curr_cell = null;

        if (questType != "DFS") //the algorithm is either FIFO or BFS
        {
            //revert properties of next cell
            GameObject next_cell = GameObject.Find((num_ing - capacity).ToString());
            next_cell.GetComponent<QCellAssigner>().isRear = false;

            //revert properties of current cell
            curr_cell = GameObject.Find((num_ing - (capacity - 1)).ToString());
            curr_cell.GetComponent<QCellAssigner>().isRear = true;
            curr_cell.GetComponent<Image>().color = canvasScript.originalColor;

            //move the rear pointer one step back
            Vector2 rearPos = GameObject.Find("rear").GetComponent<RectTransform>().anchoredPosition;
            GameObject.Find("rear").GetComponent<RectTransform>().anchoredPosition = new Vector2(rearPos.x + 85f, rearPos.y);

            //restore ingredient slot properties
            if (questType == "BFS") //if the algorithm is BFS
            {
                Dictionary<string, int> levelIngCount = canvasGO.GetComponent<SpawnIngredient>().levelIngCount; //get count of ingredients in each BFS level

                //if the current BFS level is not equal to previous BFS level
                if (canvasGO.GetComponent<SpawnIngredient>().currLevel != canvasGO.GetComponent<SpawnIngredient>().prevLevel)
                {
                    HighlightPreviousLevel(canvasGO.GetComponent<SpawnIngredient>().currLevel); //highlight the previous level
                    canvasGO.GetComponent<SpawnIngredient>().currLevel--; //move back to previous level
                    levelIngCount[canvasGO.GetComponent<SpawnIngredient>().currLevel.ToString()]++; //increase previous level count by 1
                }
                else //if current BFS level is equal to previous BFS level
                {
                    levelIngCount[canvasGO.GetComponent<SpawnIngredient>().currLevel.ToString()]++; //increase current level count by 1
                }
            }
        }
        else //the algorithm is DFS
        {
            //revert properties of next cell
            GameObject next_cell = GameObject.Find((capacity + 1).ToString());
            next_cell.GetComponent<SCellAssigner>().isTop = false;

            //revert properties of current cell
            curr_cell = GameObject.Find((capacity).ToString());
            curr_cell.GetComponent<SCellAssigner>().isTop = true;
            curr_cell.GetComponent<Image>().color = canvasScript.originalColor;

            //move the top pointer one step back
            Vector2 topPos = GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition;
            GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition = new Vector2(topPos.x , topPos.y - 85f);

            GameObject currIngredient = GetDroppedIngredient(curr_cell, ingredientSlots); //get the ingredient dropped in the current cell
            UnHighlightNeighbors(currIngredient); //unhighlight its neighbors
            if (capacity != 1) //if the stack capacity is not 1 i.e. not the source ingredient
            {
                GameObject prevIngredient = GetDroppedIngredient(GameObject.Find((capacity - 1).ToString()), ingredientSlots); //get the ingredient of the previous stack cell
                HighlightNonVisitedNeighbors(prevIngredient); //highlight its non-visited neighbors
            }
            else //the starting cell of the stack
            {
                GameObject.Find("_" + canvasGO.GetComponent<SpawnIngredient>().source_node).GetComponent<Image>().color = Color.yellow; //highlight the source ingredient
            }
            //reduce top index
            canvasGO.GetComponent<SpawnIngredient>().topIndex--;
        }

        canvasScript.capacity--; //decrease the capacity of the queue/stack

        //restore the wrong placed ingredient to its original position and add the 'DragAndDrop' script to all such ingredients not in the queue/stack
        GameObject droppedIngredient = GetDroppedIngredient(curr_cell, ingredientSlots);
        ingredientSlots[droppedIngredient] = null;
        if (questType == "FIFO") //for FIFO
        {
            droppedIngredient.tag = "Untagged"; //it would be Untagged
            DragAndDrop dragScript = droppedIngredient.AddComponent<DragAndDrop>();
        }
        else //for DFS, BFS
            droppedIngredient.tag = "Node"; //it would be a graph Node
        droppedIngredient.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("_" + droppedIngredient.name).GetComponent<RectTransform>().anchoredPosition;
        droppedIngredient.transform.SetParent(GameObject.Find("_" + droppedIngredient.name).transform);
        droppedIngredient.GetComponent<CanvasGroup>().blocksRaycasts = true;
        droppedIngredient.GetComponent<CanvasGroup>().alpha = 1f;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject obj in objs)
        {
            if(obj.GetComponent<DragAndDrop>() == null)
            { 
                DragAndDrop dragScript = obj.AddComponent<DragAndDrop>();
                Debug.Log("");
            }
        }
        
        //disable the revert button
        GetComponent<Button>().gameObject.SetActive(false);
    }
    /* function to highlight the non-visited neighbors (specifically their ingredient slots) of the dropped ingredient */
    void HighlightNonVisitedNeighbors(GameObject droppedIngredient)
    {
        List<Node> ingNeighbors = droppedIngredient.GetComponent<Node>().neighbors;
        HashSet<string> dfsVisited = canvasGO.GetComponent<SpawnIngredient>().dfsVisited;

        foreach (Node neigh in ingNeighbors)
        {
            if (!dfsVisited.Contains(neigh.gameObject.name))
                GameObject.Find("_" + neigh.gameObject.name).GetComponent<Image>().color = Color.yellow;
        }
    }
    /* function to unhighlight the neighbors of the dropped ingredient */
    void UnHighlightNeighbors(GameObject droppedIngredient)
    {
        List<Node> ingNeighbors = droppedIngredient.GetComponent<Node>().neighbors;
   
        foreach (Node neigh in ingNeighbors)
        {
            GameObject.Find("_" + neigh.gameObject.name).GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor;
        }
    }
    /* function to highlight the previous level of ingredients (specifically their ingredient slots) in the graph for BFS*/
    void HighlightPreviousLevel(int level)
    {
        ItemSlotAssigner[] gameObjs = GameObject.FindObjectsOfType<ItemSlotAssigner>();

        foreach (ItemSlotAssigner obj in gameObjs)
        {
            if (obj.cellID == (level - 1).ToString()) //highlight the previous level
            {
                obj.gameObject.GetComponent<Image>().color = Color.yellow;
            }
            if (obj.cellID == (level).ToString()) //restore the original color of the current level
            {
                obj.gameObject.GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor; 
            }
        }
    }
    /* function to get the ingredient that was dropped in the cell */
    public GameObject GetDroppedIngredient(GameObject qCell, Dictionary<GameObject,GameObject> ingredientSlots)
    {
        GameObject key = null;
        foreach (KeyValuePair<GameObject, GameObject> ele in ingredientSlots)
        {
            if (ele.Value == qCell)
            {
                key = ele.Key;
                break;
            }
        }
        return key;
    }
}
