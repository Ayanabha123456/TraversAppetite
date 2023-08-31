using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* class to initialize the fridge scene for LIFO */
public class StoreIngredients : MonoBehaviour
{
    //paths to load various sprites
    private string ingredientsPath = "Ingredients";
    private string cellPath = "Cells/ItemSlot";
    private string pointersPath = "Pointers";
    //ingredient sprites and their corresponding indices
    private Sprite[] ingredientSprites;
    private List<int> ingIndices = new List<int>();
    private int ingCount = 0; //index for each ingredient
    public int numOfIngredients = 10;
    public int capacity = 0; //capacity of stack
    //dimensions of rendered ingredients
    public int ingWidth = 50;
    public int ingHeight = 50;
    //Note UI
    public GameObject noteText;
    public GameObject note;
    public string listText;
    public string infoText;
    public Button finish; //finish button
    public GameObject promptGO; //the text prompt
    public List<GameObject> ingredients = new List<GameObject>(); //list of ingredients
    public Dictionary<string, int> ingredientDict = new Dictionary<string, int>();//index for each ingredient
    public Dictionary<GameObject, GameObject> ingredientSlots = new Dictionary<GameObject, GameObject>(); //stores the current stack cell at which each ingredient is
    public Dictionary<GameObject, Vector2> positions = new Dictionary<GameObject, Vector2>(); //store original positions of spawned ingredients

    void Start()
    {
        //disable finish button
        finish.gameObject.SetActive(false);

        //show starting prompt
        DisplayPrompt("Top pointer at the bottom. Stack is empty. Push ingredients at top pointer.");

        //load all the sprites from the 'Ingredients' folder
        ingredientSprites = Resources.LoadAll<Sprite>(ingredientsPath);
        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            ingredientDict.Add(ingredientSprites[i].name, i);
        }
        //pick different ingredient indices
        foreach (string ingredient in ManagerScript.Instance.recipe)
        {
            ingIndices.Add(ingredientDict[ingredient]);
        }

        //initialize the note UI gameobject
        note = GameObject.Find("Note");
        noteText = note.transform.GetChild(0).gameObject;
        noteText.GetComponent<TextMeshProUGUI>().text = "";

        GenerateStackCells();
        GenerateStreamIngredient();

        note.transform.SetAsLastSibling(); //this makes the note ui appear on top of all other ui elements when opened
        SetNoteText();
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    /* this function generates the stack cells for LIFO */
    void GenerateStackCells()
    {
        //load the Stack Cell Prefab
        Sprite spriteCell = Resources.Load<Sprite>(cellPath);

        //load the Stack Pointer Prefab
        Sprite pointer = Resources.LoadAll<Sprite>(pointersPath)[0];

        float cellY = 100f;
        int sCount = 0;

        for (int i = 1; i <= 10; i++)
        {
            sCount++;
            
            //create a stack cell  and generate it on a random position of the screen
            Vector2 randomPositionCell = new Vector2(750f, cellY);
            cellY += 85f;
            GameObject spawnedCell = new GameObject(sCount.ToString());
            spawnedCell.transform.SetParent(transform);
            spawnedCell.transform.position = randomPositionCell;
            Image qComponent = spawnedCell.AddComponent<Image>();
            qComponent.sprite = spriteCell;

            //assign the 'drop' logic on the stack cell
            StackDrop dropScript = spawnedCell.AddComponent<StackDrop>();

            //assign the 'SCellAssigner' identifier script to the queue cell
            SCellAssigner sCellScript = spawnedCell.AddComponent<SCellAssigner>();

            if (sCount == 1) //set top pointer for first cell
            {
                sCellScript.isTop = true;

                GameObject topPointer = new GameObject("top");
                topPointer.transform.SetParent(transform);
                topPointer.transform.position = new Vector2(695f, cellY - 85f);
                Image tComponent = topPointer.AddComponent<Image>();
                tComponent.sprite = pointer;
                topPointer.GetComponent<RectTransform>().sizeDelta = new Vector2(ingWidth, ingHeight);
            }

        }
    }
    /* this function spawns the ingredients for LIFO one at a time */
    public void GenerateStreamIngredient()
    {
        //pick the ingredient sprite
        Sprite spawnedSprite = ingredientSprites[ingIndices[ingCount]];

        //create a game-object corresponding to the sprite and set its parent as the Canvas and generate it at the position of the Item Slot
        Vector2 randomPositionIng = GameObject.Find("ItemSlot").transform.position;
        GameObject ing = new GameObject(spawnedSprite.name);
        ing.transform.position = randomPositionIng;
        ing.transform.SetParent(transform);

        //add an entry for the ingredient's original position
        positions.Add(ing, randomPositionIng);

        //add a hovering 'Text' Component for the ingredient
        GameObject ingText = new GameObject("T:" + spawnedSprite.name);
        TextMeshProUGUI text = ingText.AddComponent<TextMeshProUGUI>();
        text.text = ing.name;
        text.gameObject.SetActive(false);
        ingText.transform.position = ing.transform.position;
        ingText.transform.SetParent(ing.transform);

        //assign the sprite image to its Image Component
        Image imgComponent = ing.AddComponent<Image>();
        imgComponent.sprite = spawnedSprite;

        //add a CanvasGroup component to the spawned sprite so that it can be detected by the stack cells
        CanvasGroup ingCanvasGroup = ing.AddComponent<CanvasGroup>();

        //set the width and height of the spawned ingredient
        RectTransform ingredientTransform = ing.GetComponent<RectTransform>();
        ingredientTransform.sizeDelta = new Vector2(ingWidth,ingHeight);

        //assign the Stack-Drag logic/script to the spawned ingredient
        StackDrag dragScript = ing.AddComponent<StackDrag>();

        //add to list of ingredients
        ingredients.Add(ing);

        //create an entry (ingredient, cell in which it is dropped)
        ingredientSlots.Add(ing, null);

        //increment the ingredient index count
        ingCount++;

    }
    /* function to set the text to be displayed for the note UI */
    void SetNoteText()
    {
        //set the list text
        noteText.GetComponent<TextMeshProUGUI>().text = "This is the list of ingredients for the dish - " + ManagerScript.Instance.dish + " : ";

        foreach (int ingId in ingIndices)
        {
            Sprite spawnedSprite = ingredientSprites[ingId];
            noteText.GetComponent<TextMeshProUGUI>().text += spawnedSprite.name + ", ";
        }

        noteText.GetComponent<TextMeshProUGUI>().text += "Drag and Drop the ingredients as they appear at the top of the stack.";
        listText = noteText.GetComponent<TextMeshProUGUI>().text;

        //set the algorithm info
        infoText = "Last-In-First-Out is a linear traversal technique where you add elements at the top of the stack and remove elements from the top of the stack as well.";
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        promptGO.SetActive(false);
    }
    void Update()
    {
        //when the stack is full, attach the 'StackPopDrag' logic to the ingredients
        if (capacity == 10)
        {
            //show stack full prompt
            DisplayPrompt("Stack is full. Pop ingredients from top pointer and drop in the fridge.");
            
            GameObject.Find("10").GetComponent<SCellAssigner>().isTop = true;

            GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Processed");
            
            foreach (GameObject ing in ingredients)
            {
                StackPopDrag spdrag = ing.AddComponent<StackPopDrag>();
            }
            capacity += 1;

            //also remove the 'StackDrop' component on the stack cells
            for (int i = 1; i <= 10; i++)
            {
                GameObject sCell = GameObject.Find(i.ToString());
                Destroy(sCell.GetComponent<StackDrop>());
            }
        }
        //sanity-check to remove 'Stack Drag' component from all ingredients
        if(capacity == 11)
        {
            capacity += 1;
            StackDrag[] leftIng = GameObject.FindObjectsOfType<StackDrag>();
            foreach(StackDrag ing in leftIng)
            {
                Destroy(ing.gameObject.GetComponent<StackDrag>());
            }
        }
    }
}
