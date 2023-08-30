using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* a class that is used for selecting one of the options of the MCQ */
public class SelectOption : MonoBehaviour
{
    private Button optionButton;
    private GameObject canvasGO;
    // Start is called before the first frame update
    void Start()
    {
        canvasGO = GameObject.Find("Canvas");
        optionButton = GetComponent<Button>(); //the option that is selected
        optionButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        //render all the option slots to their original color;
        for(int i = 0; i < 4; i++)
        {
            canvasGO.GetComponent<SetQuestion>().Options.transform.GetChild(i).tag = "Option";
            canvasGO.GetComponent<SetQuestion>().Options.transform.GetChild(i).GetComponent<Image>().color = canvasGO.GetComponent<SetQuestion>().originalColor;
        }

        //change color of selected option to light blue and change the tag to 'Selected'
        optionButton.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.7f, 1.0f, 1.0f);
        optionButton.gameObject.tag = "Selected";

        //enable the 'Submit' button
        canvasGO.GetComponent<SetQuestion>().Submit.GetComponent<Button>().interactable = true;
    }
}
