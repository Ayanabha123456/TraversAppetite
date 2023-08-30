using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* class to initialize the cooking scene for FIFO/BFS/DFS */
public class SpawnIngredient : MonoBehaviour
{
    //paths to load various sprites
    private string ingredientsPath = "Ingredients";
    private string cellPath = "Cells/ItemSlot";
    private string edgePath = "Edges/edge";
    private string pointersPath = "Pointers";
    private string slotPath = "Slots/circle";
    private Recipes rec; //the predefined recipes
    //ingredient sprites and their corresponding indices
    private Sprite[] ingredientSprites;
    private List<int> ingIndices = new List<int>();
    //where to render the ingredients
    private float ingX = 100f;
    private float ingY = 250f;
    private int ingCount = 0; //index for each ingredient in the graph
    private float radius = 200f; //radius for the graph of ingredients
    public string questType; //type of algorithm
    public string source_node; //the source node for either BFS or DFS
    public int topIndex = 1; //the index of the top pointer for DFS
    public int numOfIngredients = 10;
    public int ingLocation; //index for storage slot in Pan
    //color of the queue/stack cells and the item slots
    public Color originalColor;
    public Color slotOriginalColor;
    public GameObject promptGO; //the text prompt
    //interaction buttons
    public Button revert;
    public Button backtrack;
    public Button finish;
    public int capacity = 0; //capacity of stack/queue
    //dimensions of rendered ingredients
    public int ingWidth = 50;
    public int ingHeight = 50;
    //variables to indicate levels in BFS
    public int prevLevel = 0;
    public int currLevel = 0;
    //Note UI
    public GameObject note;
    public GameObject noteText;
    public string recipeText;
    public string infoText;
    public HashSet<string> dfsVisited = new HashSet<string>(); // visited DS for DFS
    public List<GameObject> ingredients = new List<GameObject>(); //list of ingredients
    public Dictionary<string, List<string>> adj = new Dictionary<string, List<string>>(); //adjacency list for graph
    public Dictionary<string, int> ingredientDict = new Dictionary<string, int>();//index for each ingredient
    public Dictionary<GameObject, GameObject> ingredientSlots = new Dictionary<GameObject, GameObject>(); //stores the current queue/stack cell in which each ingredient is
    public Dictionary<GameObject, Vector2> positions = new Dictionary<GameObject, Vector2>(); //store original positions of spawned ingredients
    public Dictionary<string, int> bfsResult = new Dictionary<string, int>(); //store the bfs result (node, level)
    public Dictionary<string, int> levelIngCount = new Dictionary<string, int>(); //store the no. of elements at each level of BFS

    // Start is called before the first frame update
    void Start()
    {
        //get the algorithm
        questType = ManagerScript.Instance.questType;

        //get the 'Recipes' component and the recipe for the dish to be made
        rec = GetComponent<Recipes>();
        string[] recipe = ManagerScript.Instance.recipe;

        //make the backtrack button non-interactable
        backtrack.interactable = false;

        //show starting prompt
        if (questType != "DFS")
        {
            //disable the backtrack button
            backtrack.gameObject.SetActive(false);

            DisplayPrompt("Front and rear pointers at same position. Queue is empty. Enqueue ingredients at rear pointer.");
        }
        else
        {
            DisplayPrompt("Top pointer at the bottom. Stack is empty. Push ingredients at top pointer.");
        }

        //disable the revert button
        revert.gameObject.SetActive(false);

        //disable finish button
        finish.gameObject.SetActive(false);

        //load all the sprites from the 'Ingredients' folder
        ingredientSprites = Resources.LoadAll<Sprite>(ingredientsPath);
        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            ingredientDict.Add(ingredientSprites[i].name, i);
        }
        //pick different ingredient indices
        foreach (string ingredient in recipe)
        {
            ingIndices.Add(ingredientDict[ingredient]);
        }

        //initialize the note UI gameobject
        note = GameObject.Find("Note");
        noteText = note.transform.GetChild(0).gameObject;
        noteText.GetComponent<TextMeshProUGUI>().text = "";

        GenerateCells();

        //set-up the source node for BFS/DFS
        if (questType == "DFS")
            source_node = ingredientSprites[ingIndices[Random.Range(0, ingIndices.Count)]].name;
        else
            source_node = ingredientSprites[ingIndices[0]].name;

        GenerateIngredients();

        if (questType != "FIFO")
        {
            //create the edges for the graph-based algorithms
            CreateEdges();
            if (questType == "BFS") //run BFS
            {
                FindBFS(10);
            }
        }
        note.transform.SetAsLastSibling(); //this makes the note ui appear on top of all other ui elements when opened
        SetNoteText();
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    /* this function generates the queue/stack cells */
    void GenerateCells()
    {
        //load the Cell Prefab
        Sprite spriteCell = Resources.Load<Sprite>(cellPath);

        //load the Pointers Prefab
        Sprite[] pointers = Resources.LoadAll<Sprite>(pointersPath);

        float cellX = 190f; //for queue cells
        float cellY = 100f; //for stack cells

        int cCount = 0;

        for (int i = ingIndices.Count - 1; i >= 0; i--)
        {
            cCount++;
            //pick the ingredient sprite
            Sprite spawnedSprite = ingredientSprites[ingIndices[i]];

            //create a cell and generate it on a random position of the screen
            Vector2 randomPositionCell = new Vector2(0f,0f);
            GameObject spawnedCell = new GameObject(cCount.ToString());
            spawnedCell.transform.SetParent(transform);

            if (questType != "DFS") //for FIFO and BFS : queue-based algorithms
            {
                //generate cells horizontally
                randomPositionCell = new Vector2(cellX, 100f);
                cellX += 85f;
                spawnedCell.transform.position = randomPositionCell;

                //assign the 'drop' logic on the queue cell
                Drop dropScript = spawnedCell.AddComponent<Drop>();

                //assign the 'QCellAssigner' identifier script to the queue cell
                QCellAssigner qCellScript = spawnedCell.AddComponent<QCellAssigner>();

                if (questType == "FIFO") //for FIFO, set the cellID to the ingredient that occurs sequentially in the recipe
                    qCellScript.cellID = spawnedSprite.name;

                if (cCount == numOfIngredients) //set front and rear pointers for first cell
                {
                    qCellScript.isFront = true;
                    qCellScript.isRear = true;

                    GameObject frontPointer = new GameObject("front");
                    frontPointer.transform.SetParent(transform);
                    frontPointer.transform.position = new Vector2(cellX - 85f, 150f);
                    Image fComponent = frontPointer.AddComponent<Image>();
                    fComponent.sprite = pointers[2];
                    frontPointer.GetComponent<RectTransform>().sizeDelta = new Vector2(ingWidth, ingHeight);

                    GameObject rearPointer = new GameObject("rear");
                    rearPointer.transform.SetParent(transform);
                    rearPointer.transform.position = new Vector2(cellX - 85f, 50f);
                    Image rComponent = rearPointer.AddComponent<Image>();
                    rComponent.sprite = pointers[1];
                    rearPointer.GetComponent<RectTransform>().sizeDelta = new Vector2(ingWidth, ingHeight);
                }
            }
            else // for DFS : stack-based algorithm
            {
                //generate cells vertically
                randomPositionCell = new Vector2(750f, cellY);
                cellY += 85f;
                spawnedCell.transform.position = randomPositionCell;

                //assign the 'drop' logic on the stack cell
                DFSDrop dropScript = spawnedCell.AddComponent<DFSDrop>();

                //assign the 'SCellAssigner' identifier script to the stack cell
                SCellAssigner sCellScript = spawnedCell.AddComponent<SCellAssigner>();

                if (cCount == 1) //set top pointer for first cell
                {
                    sCellScript.isTop = true;

                    GameObject topPointer = new GameObject("top");
                    topPointer.transform.SetParent(transform);
                    topPointer.transform.position = new Vector2(700f, cellY - 85f);
                    Image tComponent = topPointer.AddComponent<Image>();
                    tComponent.sprite = pointers[0];
                    topPointer.GetComponent<RectTransform>().sizeDelta = new Vector2(ingWidth, ingHeight);
                }
            }
            
            //assign the Cell prefab to the generated cell and store its original color
            Image cComponent = spawnedCell.AddComponent<Image>();
            cComponent.sprite = spriteCell;
            originalColor = cComponent.color;

        }
    }
    /* function to get the position in which the ingredient will be spawned */
    Vector2 GetSpawnPosition()
    {
        Vector2 spawnPos = new Vector2(0f, 0f);
        if(questType == "FIFO") // for FIFO, spawn the ingredients in two rows of 5 ingredients each
        {
            spawnPos = new Vector2(ingX, ingY);

            //modifying the ingredient (X,Y) based on count
            ingCount++;
            if (ingCount == ingIndices.Count / 2)
            {
                ingX = 100f;
                ingY = 350f;
            }
            else
            {
                ingX += 100f;
            }

        }
        else if(questType == "BFS" || questType == "DFS") //for BFS and DFS, spawn the ingredients around the circumference of a circle with Centre and radius
        {
            float angleStep = 360f / numOfIngredients;
            float angle = ingCount * angleStep;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            spawnPos = GameObject.Find("Centre").transform.position + new Vector3(x, y, 0);

            ingCount++;
        }

        return spawnPos;
    }
    void GenerateIngredients()
    {
        //load the Ingredient Slot Prefab
        Sprite slot = Resources.Load<Sprite>(slotPath);

        for (int i= ingIndices.Count - 1; i >= 0; i--)
        {
            //pick the ingredient sprite
            Sprite spawnedSprite = ingredientSprites[ingIndices[i]];

            //create a slot for the ingredient
            GameObject ingSlot = new GameObject("_"+spawnedSprite.name);
            ItemSlotAssigner slotScript = ingSlot.AddComponent<ItemSlotAssigner>();
            Image imgSlotComp = ingSlot.AddComponent<Image>();
            imgSlotComp.sprite = slot;
            slotOriginalColor = imgSlotComp.color;

            if (questType != "FIFO" && spawnedSprite.name == source_node) //for graph-based algorithm highlight the slot of the source ingredient
                imgSlotComp.color = Color.yellow;

            //create a game-object corresponding to the sprite, set its parent as the Canvas and generate it on a random position of the screen
            Vector2 randomPositionIng = GetSpawnPosition();
            GameObject ing = new GameObject(spawnedSprite.name);
            ing.transform.position = randomPositionIng;
            ingSlot.transform.position = randomPositionIng;
            ingSlot.transform.SetParent(transform);
            ing.transform.SetParent(ingSlot.transform);

            //add an entry for the ingredient's original position
            positions.Add(ing, randomPositionIng);

            //add a hovering 'Text' Component for the ingredient
            GameObject ingText = new GameObject("T:"+spawnedSprite.name);
            TextMeshProUGUI text = ingText.AddComponent<TextMeshProUGUI>();
            text.text = ing.name;
            text.gameObject.SetActive(false);
            ingText.transform.position = ing.transform.position;
            ingText.transform.SetParent(ing.transform);

            //assign the sprite image to its Image Component
            Image imgComponent = ing.AddComponent<Image>();
            imgComponent.sprite = spawnedSprite;

            //add a CanvasGroup component to the spawned sprite so that it can be detected by the queue/stack cells
            CanvasGroup ingCanvasGroup = ing.AddComponent<CanvasGroup>();

            //set the width and height of the spawned ingredient
            RectTransform ingredientTransform = ing.GetComponent<RectTransform>();
            ingredientTransform.sizeDelta = new Vector2(ingWidth, ingHeight);

            //assign the DragAndDrop logic/script to the spawned ingredient
            DragAndDrop dragScript = ing.AddComponent<DragAndDrop>();
            
            if (questType == "BFS" || questType == "DFS") //for graph-based algorithm, make the ingredient a Node as well
            {
                //add the 'Node' script to make it a graph node
                Node node = ing.AddComponent<Node>();
                node.position = randomPositionIng;
                node.nodeName = spawnedSprite.name;
                ing.tag = "Node";
            }

            //add to list of ingredients
            ingredients.Add(ing);

            //create an entry (ingredient, cell in which it is dropped)
            ingredientSlots.Add(ing, null);
        }
    }
    /* function to add edges to the graph of ingredients */
    void CreateEdges()
    {
        //Load the edge sprite
        Sprite edgeSprite = Resources.Load<Sprite>(edgePath);

        /*creating edges so that graph is connected*/
        //Step 1 - create an empty adjacency list
        List<string> keys = new List<string>();

        foreach (GameObject ob in ingredients)
        {
            keys.Add(ob.name);
            adj.Add(ob.name, new List<string>());
        }

        //Step 2 - get the list of edges
        List<List<string>> edges = new List<List<string>>();
        if (questType == "DFS")
        {
            //for DFS get the edges through Kruskal's algorithm
            AddEdges obj = new AddEdges(keys);
            edges = obj.Kruskal();
        }
        else
        {
            //for BFS, get predefined edges in the Recipes component
            Dictionary<string, List<List<string>>> recipeEdges = rec.recipeEdges;
            edges = recipeEdges[ManagerScript.Instance.dish];
        }
        
        //Step 3 - render the edges
        int edge_counter = 1;

        foreach (List<string> ll in edges)
        {

            RectTransform v1 = GameObject.Find(ll[0]).GetComponent<RectTransform>();
            RectTransform v2 = GameObject.Find(ll[1]).GetComponent<RectTransform>();

            //create the edge
            GameObject edge = new GameObject("edge" + (edge_counter).ToString());
            edge.layer = 5;
            edge.transform.SetParent(transform);
            Image edgeImage = edge.AddComponent<Image>();
            edgeImage.sprite = edgeSprite;

            //align the edge between the two ingredients
            Vector2 midpoint = (v1.position + v2.position) / 2f;
            edgeImage.transform.position = midpoint;

            Vector2 direction = (v2.position - v1.position).normalized;
            float distance = Vector2.Distance(v1.position, v2.position);
            edgeImage.rectTransform.sizeDelta = new Vector2(distance, 25f);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            edgeImage.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            //render the edge below the ingredients
            if(questType == "BFS")
                edge.transform.SetSiblingIndex(GameObject.Find("rear").transform.GetSiblingIndex() + edge_counter);
            else
                edge.transform.SetSiblingIndex(GameObject.Find("top").transform.GetSiblingIndex() + edge_counter);

            edge_counter += 1;
            //add edge data to the list of neighbors
            Node n1 = GameObject.Find(ll[0]).GetComponent<Node>();
            Node n2 = GameObject.Find(ll[1]).GetComponent<Node>();

            n1.neighbors.Add(n2);
            n2.neighbors.Add(n1);

            //add data to the culminated adjacency list
            adj[ll[0]].Add(ll[1]);
            adj[ll[1]].Add(ll[0]);

        }
    }
    /* function to initiate the BFS for the graph of ingredients */
    void FindBFS(int qCount)
    {
        //call the BFS functionality with the adjacency list
        BFS bfs = new BFS(adj);
        bfsResult = bfs.BfsWithLevels(source_node);

        //iterate through the BFS result 
        foreach (KeyValuePair<string, int> kv in bfsResult)
        {
            //assign the BFS levels as the cellIDs of the queue cells
            GameObject qCell = GameObject.Find(qCount.ToString());
            qCell.GetComponent<QCellAssigner>().cellID = kv.Value.ToString();
            qCount--;
            //assign the BFS levels as the cellIDs of the ingredient slots
            GameObject.Find("_"+kv.Key).GetComponent<ItemSlotAssigner>().cellID = kv.Value.ToString();

            //store the count of ingredients for each BFS level
            if (!levelIngCount.ContainsKey(kv.Value.ToString()))
                levelIngCount.Add(kv.Value.ToString(), 1);
            else
                levelIngCount[kv.Value.ToString()]++;
        }
    }
    /* function to set the text to be displayed for the note UI */
    void SetNoteText()
    {
        if(questType == "FIFO")
        {
            //set the recipe text
            noteText.GetComponent<TextMeshProUGUI>().text = "This dish will be made using First-In-First-Out (FIFO). Drag and Drop the ingredients in the queue in the following order: ";

            foreach (int ingId in ingIndices)
            {
                Sprite spawnedSprite = ingredientSprites[ingId];
                noteText.GetComponent<TextMeshProUGUI>().text += spawnedSprite.name + ", ";
            }

            //set the algorithm info
            infoText = "First-In-First-Out is a linear traversal technique where you add elements at the rear of the queue and remove elements from the front of the queue.";
        }
        else if(questType == "BFS")
        {
            //set the recipe text
            noteText.GetComponent<TextMeshProUGUI>().text = "This dish will be made using Breadth First Search (BFS). Drag the Drop the ingredients in the queue starting from the source ingredient - "+source_node+". Then go for the other ingredients in the following order of levels: ";
            int prev = 0;
            foreach(KeyValuePair<string, int> kv in bfsResult)
            {
                int curr = kv.Value;
                if (kv.Value != 0)
                {
                    if (curr != prev)
                    {
                        noteText.GetComponent<TextMeshProUGUI>().text += ") Level " + curr + "( " + kv.Key + ", ";
                        prev = curr;
                    }
                    else
                    {
                        noteText.GetComponent<TextMeshProUGUI>().text += kv.Key + ", ";
                    }
                }
            }
            noteText.GetComponent<TextMeshProUGUI>().text += " )";

            //set the algorithm info
            infoText = "Breadth First Search is a non-linear traversal technique where you are given a graph of elements connected by edges." +
                " You add elements to the queue starting from the source element and then the other elements connected by edges in increasing order of levels. " +
                "Therefore, level 1 elements which 1 edge away from the source are added, followed by level 2 elements and so on, until all the elements in the graph are added to the queue.";
        }
        else
        {
            //set the recipe text
            noteText.GetComponent<TextMeshProUGUI>().text = "This dish will be made using Depth First Search (DFS). Drag and Drop the ingredients in the stack starting from the source ingredient - "+source_node+
                ". Then go for its subsequent neighboring ingredients as highlighted. If there are no subsequent neighbors, backtrack to the ingredient which has subsequent neighbors.";

            //set the algorithm info
            infoText = "Depth First Search is a non-linear traversal technique where you are given a graph of elements connected by edges. " +
                "You add elements to the stack starting from the source element. Then you add one of the neighbors of the source, followed by the subsequent addition of one neighbor of the current element and so on. " +
                "If the currently added element has no neighbor, backtracking takes place, that is, elements are removed from the stack until the top element has a neighbor that is not visited yet. " +
                "The traversal is the order in which all the elements are added to the stack.";
        }
        recipeText = noteText.GetComponent<TextMeshProUGUI>().text;
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for the specified display time
        yield return new WaitForSeconds(5f);

        promptGO.SetActive(false);
    }
    void Update()
    {
        //when the queue is full, attach the 'DequeueDrag' logic to the ingredients
        if (capacity == 10)
        {
            //show queue full prompt
            DisplayPrompt("Queue is full. Dequeue ingredients from front pointer and drop in the pan.");
            
            GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Processed");
            Debug.Log(ingredients.Length);
            foreach(GameObject ing in  ingredients)
            {
                DequeueDrag ddrag = ing.AddComponent<DequeueDrag>();
            }
            capacity += 1;
            
            //also remove the 'Drop' component on the queue cells
            for(int i=1;i<=10;i++)
            {
                GameObject qCell = GameObject.Find(i.ToString());
                Destroy(qCell.GetComponent<Drop>());
            }
        }
        //sanity-check to remove 'Drag and Drop' component from all ingredients
        if (capacity == 11)
        {
            capacity += 1;
            DragAndDrop[] leftIng = GameObject.FindObjectsOfType<DragAndDrop>();
            foreach (DragAndDrop ing in leftIng)
            {
                Destroy(ing.gameObject.GetComponent<DragAndDrop>());
            }
        }
    }

}
