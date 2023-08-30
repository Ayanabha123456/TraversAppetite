using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* class that initiates the Backtracking operation in DFS when the 'Backtrack' button is clicked */
public class Backtrack : MonoBehaviour
{
    private GameObject canvasGO;
    private GameObject promptGO;
    private int topIndex; //index to indicate the stack cell that is currently the top
    private Dictionary<string, List<string>> adj; //adjacency list of ingredients graph
    private Dictionary<GameObject, GameObject> ingredientSlots; //dictionary that tells which ingredient is dropped in which stack cell
    private HashSet<string> dfsVisited; //data structure to tell which ingredients are visited in DFS
    private GameObject backtrackedIngredient; //the ingredient that is backtracked to at each iteration
    
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        canvasGO = GameObject.Find("Canvas");
        promptGO = canvasGO.GetComponent<SpawnIngredient>().promptGO;
    }

    public void OnButtonClick()
    {
        topIndex = canvasGO.GetComponent<SpawnIngredient>().topIndex;

        ingredientSlots = canvasGO.GetComponent<SpawnIngredient>().ingredientSlots;
        adj = canvasGO.GetComponent<SpawnIngredient>().adj;
        dfsVisited = canvasGO.GetComponent<SpawnIngredient>().dfsVisited;
        backtrackedIngredient = GameObject.Find((topIndex - 1).ToString()).GetComponent<DFSDrop>().droppedIngredient;

        StartCoroutine(RepeatStuff()); //start the backtracking process
    }

    //while the backtracking is in progress, the player can't drag the other ingredients in the graph
    public IEnumerator RepeatStuff()
    {
        //disable the 'Backtrack' button
        GetComponent<Button>().interactable = false;

        //set the prompt to show that backtracking is in progress
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Backtracking in progress.";

        //backtrack while the current top ingredient does not have non-visited neighbors
        while (!HasNonVisitedNeighbors(backtrackedIngredient))
        {
            GoBack();
            HighlightNonVisitedNeighbors(backtrackedIngredient); //highlight the non-visited neighbors after backtracking

            yield return new WaitForSeconds(5f); //wait for 5 seconds for the next backtracking iteration step

        }

        //disable the prompt after backtracking
        promptGO.SetActive(false);

        //enable the 'DragAndDrop' script on the ingredients present in the graph after the backtracking progress
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject obj in objs)
        {
            DragAndDrop dfsdrag = obj.AddComponent<DragAndDrop>();
        }

    }
    /* function to highlight the non-visited neighbors (specifically the ingredient slots) of the current ingredient*/
    public void HighlightNonVisitedNeighbors(GameObject droppedIngredient)
    {
        List<Node> ingNeighbors = droppedIngredient.GetComponent<Node>().neighbors;
        HashSet<string> dfsVisited = canvasGO.GetComponent<SpawnIngredient>().dfsVisited;

        foreach (Node neigh in ingNeighbors)
        {
            if (!dfsVisited.Contains(neigh.gameObject.name))
                GameObject.Find("_" + neigh.gameObject.name).GetComponent<Image>().color = Color.yellow;
        }
    }

    //steps of the backtracking process in DFS
    public void GoBack()
    {
        //disable the top pointer for current top index
        GameObject.Find((topIndex).ToString()).GetComponent<SCellAssigner>().isTop = false;
        //enable the top pointer for the previous top index
        GameObject.Find((topIndex - 1).ToString()).GetComponent<SCellAssigner>().isTop = true;
        //restore the original color of the previous stack cell
        GameObject.Find((topIndex - 1).ToString()).GetComponent<Image>().color = canvasGO.GetComponent<SpawnIngredient>().originalColor;
        //pop the ingredient of the previous stack cell (i.e. the topmost ingredient)
        GameObject.Find((topIndex - 1).ToString()).GetComponent<DFSDrop>().droppedIngredient.SetActive(false);
        //change the ingredient slot for the backtracked (or popped) ingredient to null
        ingredientSlots[backtrackedIngredient] = null; 
        //reduce the top index
        topIndex--; 
        canvasGO.GetComponent<SpawnIngredient>().topIndex = topIndex;
        canvasGO.GetComponent<SpawnIngredient>().capacity--; //reduce the capacity of the DFS stack
        //get the next ingredient to backtrack to
        if(topIndex != 1)
            backtrackedIngredient = GameObject.Find((topIndex - 1).ToString()).GetComponent<DFSDrop>().droppedIngredient;
        //change the position of the top pointer after pop operation
        Vector2 topPos = GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition;
        GameObject.Find("top").GetComponent<RectTransform>().anchoredPosition = new Vector2(topPos.x, topPos.y - 85f);
    }
    /* function to check if the current ingredient has non-visited neighbors */
    public bool HasNonVisitedNeighbors(GameObject droppedIngredient)
    {
        foreach (string neighbor in adj[droppedIngredient.name])
        {
            if (!dfsVisited.Contains(neighbor))
                return true;
        }
        return false;
    }
}

