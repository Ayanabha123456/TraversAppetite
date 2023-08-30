using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/* a class that is used to view the note UI in different scenes */
public class ViewNote : MonoBehaviour
{
    private Button showButton;
    private GameObject canvasGO;
    public GameObject note;
    
    void Start()
    {
        showButton = GetComponent<Button>();

        //if it is the menu screen, do not show the note UI (i.e. the controls are hidden by default)
        if(SceneManager.GetActiveScene().name == "MenuScene")
            note.SetActive(false);

        canvasGO = GameObject.Find("Canvas");
        showButton.onClick.AddListener(OnButtonClick);


    }

    public void OnButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "CookingScene") //player is in the cooking scene
        {
            if (gameObject.name == "Info") //player press the algorithm info button
                note.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = canvasGO.GetComponent<SpawnIngredient>().infoText; //show info text
            else if (gameObject.name == "View Recipe") //player press the recipe button
                note.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = canvasGO.GetComponent<SpawnIngredient>().recipeText; //show recipe text
        }
        else if(SceneManager.GetActiveScene().name == "FridgeScene") //player is in the fridge scene
        {
            if (gameObject.name == "Info") //player press the algorithm info button
                note.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = canvasGO.GetComponent<StoreIngredients>().infoText; //show info text
            else if (gameObject.name == "View List") //player press the list ingredients button
                note.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = canvasGO.GetComponent<StoreIngredients>().listText; //show list text
        }
        note.SetActive(true); //enable the note UI
    }
}
