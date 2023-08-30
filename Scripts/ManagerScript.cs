using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* a global class (singleton) to access global values across different scenes */
public class ManagerScript : MonoBehaviour
{
    public static ManagerScript Instance { get; private set; }

    public int level = 1; //the player level
    public bool haveIngredients = false; //if the player have the ingredients for cooking the dish
    public string[] recipe; //the recipe for the dish
    public string dish; //the name of the dish
    public string questType; //the algorithm the player is going to learn

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /* function to set the algorithm based on player level */
    public void SetQuest()
    {
        if (level == 1)
        {
            questType = "FIFO";
        }
        else if (level == 2)
        {
            questType = "BFS";
        }
        else if (level == 3)
        {
            questType = "DFS";
        }
        else
        {
            string[] QuestType = new string[3] { "FIFO", "BFS", "DFS" };
            questType = QuestType[Random.Range(0, 3)];
        }
    }
    //set the dish
    public void SetDish(string chosenDish)
    {
        dish = chosenDish;
    }
    //set the recipe
    public void SetRecipe(string[] chosenRecipe)
    {
        recipe = chosenRecipe;
    }
}
