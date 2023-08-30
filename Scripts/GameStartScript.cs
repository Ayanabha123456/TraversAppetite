using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* a class that initializes the algorithm to be taught and the recipe, when the game starts and after each dish is cooked */
public class GameStartScript : MonoBehaviour
{
    void Start()
    {
        if (ManagerScript.Instance.questType.Length == 0) //if the algorithm is not set
        {
            ManagerScript.Instance.SetQuest(); //set the algorithm

            if (ManagerScript.Instance.questType != "DFS") //if it is not DFS
            {
                //Get a predefined recipe
                Recipes rec = GetComponent<Recipes>();
                Dictionary<string, string[]> recipes = rec.recipes;
                List<string> recipeKeys = new List<string>(recipes.Keys);
                int recipeIndex = Random.Range(0, recipeKeys.Count);
                ManagerScript.Instance.SetDish(recipeKeys[recipeIndex]);
                ManagerScript.Instance.SetRecipe(recipes[recipeKeys[recipeIndex]]);
            }
            else //if it is DFS
            {
                //Get a recipe with random ingredients
                Sprite[] ingredientSprites = Resources.LoadAll<Sprite>("Ingredients");
                List<int> ingIndices = new List<int>();

                while (ingIndices.Count < 10)
                {
                    int index = Random.Range(0, ingredientSprites.Length);

                    if (!ingIndices.Contains(index))
                    {
                        ingIndices.Add(index);
                    }
                }

                string[] recipe = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    recipe[i] = ingredientSprites[ingIndices[i]].name;
                }

                ManagerScript.Instance.SetDish("Crazy DFS Dish");
                ManagerScript.Instance.SetRecipe(recipe);
            }
            DisplayPrompt("Get ingredients for the dish from the fridge.");
        }
        else //if the algorithm is set
        {
            DisplayPrompt("Now proceed to the stove for cooking.");
        }
        GameObject.Find("Level").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + ManagerScript.Instance.level; //display the player level
        GameObject.Find("Recipe").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ManagerScript.Instance.dish; //display the dish that is to be cooked
        
    }
    public void DisplayPrompt(string promptText)
    {
        GameObject.Find("Prompt").SetActive(true);
        GameObject.Find("Prompt").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for the specified display time
        yield return new WaitForSeconds(5f);

        GameObject.Find("Prompt").SetActive(false);
    }
}
