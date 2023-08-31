using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/* a class that is used for interacting with the stove to cook the dish */
public class StoveInteraction : MonoBehaviour
{
    private KeyCode key = KeyCode.A;
    private GameObject promptGO;

    void Start()
    {
        promptGO = GameObject.Find("Prompt");
    }
    //enable the interaction option when the player reaches the stove
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.CompareTag("Player"))
        {
            Debug.Log("Press E to cook");
            key = KeyCode.E;
        }
    }
    //disable the interaction option when the player moves away from the stove
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            key = KeyCode.A;
        }
    }
    public void DisplayPrompt(string promptText)
    {
        promptGO.SetActive(true);
        promptGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = promptText;

        StartCoroutine(ShowPrompt());
    }
    private IEnumerator ShowPrompt()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        promptGO.SetActive(false);
    }
    void Update()
    {
        //player presses the interaction button
        if (key == KeyCode.E && Input.GetKeyDown(key))
        {
            if (ManagerScript.Instance.haveIngredients) //if player has the ingredients
            {
                 ManagerScript.Instance.haveIngredients = false;
                 SceneManager.LoadScene("CookingScene"); //switch to cooking scene to cook the dish
            }
            else
            {
                //else prompt the player to get the ingredients
                DisplayPrompt("Get the ingredients from the fridge first!!!");
            }
        }  
    }
}
