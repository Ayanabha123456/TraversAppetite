using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/* a class that is used for interacting with the fridge to get ingredients for cooking */
public class FridgeInteraction : MonoBehaviour
{
    private KeyCode key = KeyCode.A;
    private GameObject promptGO;

    void Start()
    {
        promptGO = GameObject.Find("Prompt");
    }
    //enable the interaction option when the player reaches the fridge
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            Debug.Log("Press E to buy");
            key = KeyCode.E;
        }
    }
    //disable the interaction option when the player moves away from the fridge
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
            if (!ManagerScript.Instance.haveIngredients) //if player do not have ingredients
            {
                ManagerScript.Instance.haveIngredients = true;
                SceneManager.LoadScene("FridgeScene"); //switch to fridge scene to get ingredients
            }
            else
            {
                //else prompt the player that they have the ingredients
                DisplayPrompt("You already have the ingredients for the dish!!");
            }
        }
    }
}
