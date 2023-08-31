using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/* class to drop ingredients in the stack cells for DFS algorithm*/
public class DFSDrop : MonoBehaviour, IDropHandler
{
    public GameObject droppedIngredient; //the dropped ingredient
    private GameObject canvasGO;
    private GameObject promptGO;
    private Image cellImage; //'Image' component of the stack cell - useful for changing its color
    private int ingLocation; //represents the storage slot in the Pan
    private Vector2 originalPosition; //original position of ingredient before drop
    private Dictionary<string, List<string>> adj; //adjacency list of the graph of ingredients
    private HashSet<string> dfsVisited; //visited elements in DFS
    private Button backtrack;
    private int correct_flag; //flag for creating a copy of the ingredient in the Pan only if its the correct ingredient in the DFS order
    
    void Start()
    {
        cellImage = GetComponent<Image>();
        canvasGO = GameObject.Find("Canvas");
        adj = canvasGO.GetComponent<SpawnIngredient>().adj;
        dfsVisited = canvasGO.GetComponent<SpawnIngredient>().dfsVisited;
        backtrack = canvasGO.GetComponent<SpawnIngredient>().backtrack;
        promptGO = GameObject.Find("Prompt");
    }

    /* function to check if dropped ingredient has non-visited neighbors */
    public bool HasNonVisitedNeighbors(GameObject droppedIngredient)
    {
        foreach (string neighbor in adj[droppedIngredient.name])
        {
            if (!dfsVisited.Contains(neighbor))
                return true;
        }
        return false;
    }

    /* function to change the color of the stack cell based on the dropped ingredient */
    public void ChangeColor(GameObject droppedIngredient)
    {
        if (gameObject.name == "1") //first stack cell
        {
            if (droppedIngredient.name == canvasGO.GetComponent<SpawnIngredient>().source_node) //if source ingredient is dropped
            {
                cellImage.color = Color.green; //change color to green
                dfsVisited.Add(droppedIngredient.name); //add dropped ingredient to visited data structure
                correct_flag = 1; //set the flag since correct ingredient is dropped
            }
            else //if any other ingredient is dropped
            {
                cellImage.color = Color.red; //change the color to red
                canvasGO.GetComponent<SpawnIngredient>().revert.gameObject.SetActive(true); //enable the revert button
                correct_flag = 0;//reset the flag since wrong ingredient is dropped

                //disable the 'DragAndDrop' logic on the ingredients in the graph since wrong ingredient is dropped and it needs to be reverted
                GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
                foreach (GameObject obj in objs)
                {
                    Destroy(obj.GetComponent<DragAndDrop>());
                }

                //set the appropriate prompt
                DisplayPrompt("Dropped ingredient is not the source/starting ingredient.");

            }
        }
        else //all the other stack cells
        {
            //get the ingredient dropped in the previous stack cell
            string previousCellName = (int.Parse(gameObject.name) - 1).ToString();
            GameObject previousCell = GameObject.Find(previousCellName);
            GameObject previousDroppedIngredient = GetDroppedIngredient(previousCell, canvasGO.GetComponent<SpawnIngredient>().ingredientSlots);

            if (adj[previousDroppedIngredient.name].Contains(droppedIngredient.name) && !dfsVisited.Contains(droppedIngredient.name)) //if the current ingredient is a neighbor of the previous ingredient and is not visited
            {
                cellImage.color = Color.green; //change the color to green
                dfsVisited.Add(droppedIngredient.name); //add dropped ingredient to visited data structure
                correct_flag = 1; //set the flag since correct ingredient is dropped
            }
            else //if any other ingredient is dropped
            {
                cellImage.color = Color.red; //change the color to red
                canvasGO.GetComponent<SpawnIngredient>().revert.gameObject.SetActive(true); //enable the revert button
                correct_flag = 0; //reset the flag since wrong ingredient is dropped

                //disable the 'DragAndDrop' logic on the ingredients in the graph since wrong ingredient is dropped and it needs to be reverted
                GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
                foreach (GameObject obj in objs)
                {
                    Destroy(obj.GetComponent<DragAndDrop>());
                }

                //set the appropriate prompt
                DisplayPrompt("Dropped ingredient is not the neighbor of the previously-dropped ingredient.");
            }
        }
    }

    /* function to get the ingredient that was dropped in the stack cell */
    public GameObject GetDroppedIngredient(GameObject qCell, Dictionary<GameObject, GameObject> ingredientSlots)
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

    /* function to highlight the non-visited neighbors (specifically their ingredient slots) of the dropped ingredient */
    public void HighlightNonVisitedNeighbors(GameObject droppedIngredient)
    {
        List<Node> ingNeighbors = droppedIngredient.GetComponent<Node>().neighbors;
        HashSet<string> dfsVisited = canvasGO.GetComponent<SpawnIngredient>().dfsVisited;

        foreach(Node neigh in ingNeighbors)
        {
            if (!dfsVisited.Contains(neigh.gameObject.name))
                GameObject.Find("_"+neigh.gameObject.name).GetComponent<Image>().color = Color.yellow;
        }
    }
    /* function to unhighlight the neighbors of the dropped ingredient */
    public void UnHighlightNeighbors(GameObject droppedIngredient)
    {
        List<Node> ingNeighbors = droppedIngredient.GetComponent<Node>().neighbors;

        foreach (Node neigh in ingNeighbors)
        {
            GameObject.Find("_" + neigh.gameObject.name).GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor;
        }
    }
    /* function to unhighlight all the graph ingredients */
    public void UnHighlightElements()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject obj in objs)
        {
            GameObject.Find("_"+obj.name).GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor;
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
        SCellAssigner sScript = gameObject.GetComponent<SCellAssigner>(); //get the SCell identifier of the cell where an element is dropped

        if (eventData.pointerDrag != null) //if an ingredient is dropped
        {
            droppedIngredient = eventData.pointerDrag.gameObject; //get the dropped ingredient
            droppedIngredient.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().gameObject.SetActive(false); //disable the hovering text
            
            //disable the revert button for the next step
            canvasGO.GetComponent<SpawnIngredient>().revert.gameObject.SetActive(false);

            if (!sScript.isTop) //if the cell is not the top of the stack
            {

                //snap back to original position of the dropped ingredient
                originalPosition = droppedIngredient.GetComponent<DragAndDrop>().originalPosition;
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);

                //display prompt text
                DisplayPrompt("Can only push at top of stack.");
            }
            else //if the cell is the top of the stack
            {
                //increase top pointer index by 1
                canvasGO.GetComponent<SpawnIngredient>().topIndex++;

                //increase capacity of stack by 1
                canvasGO.GetComponent<SpawnIngredient>().capacity++;

                //unhighlight every ingredient in the graph
                UnHighlightElements();
 
                if(gameObject.name != "1")
                {
                    GameObject prevIngredient = GameObject.Find((int.Parse(gameObject.name) - 1).ToString()).GetComponent<DFSDrop>().droppedIngredient;
                    //unhighlight the neighbors for the previous ingredient
                    UnHighlightNeighbors(prevIngredient);
                }

                //unhighlight the current ingredient and highlight the next set of neighbors for the current ingredient
                GameObject.Find("_" + droppedIngredient.name).GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().slotOriginalColor;
                HighlightNonVisitedNeighbors(droppedIngredient);

                //snap the dropped ingredient into the stack cell
                droppedIngredient.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

                //modify the (dropped game object, slot) in the dictionary
                Dictionary<GameObject, GameObject> hm = canvasGO.GetComponent<SpawnIngredient>().ingredientSlots;
                hm[droppedIngredient] = gameObject;

                //change color of stack cell
                ChangeColor(droppedIngredient);

                //change the Tag of dropped ingredient to 'Processed'
                droppedIngredient.tag = "Processed";

                //update the top pointer
                sScript.isTop = false;
                int curr_cell_num = int.Parse(gameObject.name);
                GameObject nextCell = GameObject.Find((curr_cell_num + 1).ToString());
                nextCell.GetComponent<SCellAssigner>().isTop = true;
                Vector2 topPos = GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition;
                GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition = new Vector2(topPos.x, topPos.y + 85f);

                //remove the 'DragAndDrop' script
                Destroy(droppedIngredient.GetComponent<DragAndDrop>());

                //create a copy of the dropped ingredient and drop it to the appropriate slot in the 'Pan'
                if (correct_flag == 1)
                {
                    ingLocation = canvasGO.GetComponent<SpawnIngredient>().ingLocation;
                    GameObject copyIng = Instantiate(droppedIngredient, GameObject.Find("Pan").transform.GetChild(ingLocation).position, GameObject.Find("Pan").transform.GetChild(ingLocation).rotation);
                    copyIng.transform.SetParent(GameObject.Find("Pan").transform.GetChild(ingLocation).transform);
                    canvasGO.GetComponent<SpawnIngredient>().ingLocation++;

                    //once all ingredients are dropped in pan enable finish button
                    if (canvasGO.GetComponent<SpawnIngredient>().ingLocation == 10)
                    {
                        canvasGO.GetComponent<SpawnIngredient>().finish.gameObject.SetActive(true);
                    }
                }

                if (HasNonVisitedNeighbors(droppedIngredient)) //if current ingredient has non-visited neighbors
                {
                    //do nothing
                }
                else //if all neighbors of current ingredient are visited
                {
                    backtrack.interactable = true; //enable the 'Backtrack' button

                    //disable the 'DragAndDrop' logic from the graph of ingredients since backtracking needs to done before the rest of the ingredients can be pushed into the stack
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
                    foreach (GameObject obj in objs)
                    {
                        Destroy(obj.GetComponent<DragAndDrop>());
                    }
                }

                //after drop, put the note UI on top of all UI elements
                canvasGO.GetComponent<SpawnIngredient>().note.transform.SetAsLastSibling();

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
