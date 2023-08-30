using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* class to transition between scenes in the game using UI buttons */
public class Finish : MonoBehaviour
{
    private Button finishButton;
    
    void Start()
    {
        finishButton = GetComponent<Button>();
        finishButton.onClick.AddListener(OnButtonClick);
    }
    
    public void OnButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "CookingScene") //player is in cooking scene (making the dish)
        {
            //increase player level until max level (Level 4) is reached
            if (ManagerScript.Instance.level < 4)
                ManagerScript.Instance.level++;

            SceneManager.LoadScene("MCQScene"); //transition to post-game quiz scene
        }
        else if (SceneManager.GetActiveScene().name == "FridgeScene") //player is in the fridge scene (getting ingredients for the dish)
        {
            SceneManager.LoadScene("MCQScene"); //transition to post-game quiz scene
        }
        else if (SceneManager.GetActiveScene().name == "MCQScene") //player is in the post-game quiz scene
        {
            SceneManager.LoadScene("StartScene"); //transition to start scene (where the player is in the kitchen)
        }
        else if(SceneManager.GetActiveScene().name == "MenuScene") //player is in the starting menu
        {
            if(finishButton.gameObject.name == "Play") // if the player presses the 'Play' button
            {
                SceneManager.LoadScene("StartScene"); //transition to the start scene
            }
            else if(finishButton.gameObject.name == "Exit") // if the player presses the 'Exit' button
            {
                Application.Quit(); //close the game
            }
        }
        else if(SceneManager.GetActiveScene().name == "StartScene") //player is in the start scene and presses the 'Go Back' button
        {
            //reset the algorithm and the player level
            ManagerScript.Instance.questType = "";
            ManagerScript.Instance.level = 1;
            ManagerScript.Instance.haveIngredients = false;

            SceneManager.LoadScene("MenuScene"); //transition to the starting menu
        }
    }
}
