using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* a class to submit the selected option in the MCQ - it is attached to the 'Submit' button in the MCQScene */
public class SubmitAnswer : MonoBehaviour
{
    private Button submitButton;
    private GameObject canvasGO;
    
    void Start()
    {
        canvasGO = GameObject.Find("Canvas");
        submitButton = GetComponent<Button>();
        submitButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        //disable all the options
        for (int i = 0; i < 4; i++)
        {
            canvasGO.GetComponent<SetQuestion>().Options.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }

        //if the selected option is correct i.e. matches with the correct answer
        if (GameObject.FindGameObjectsWithTag("Selected")[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == canvasGO.GetComponent<SetQuestion>().mcq.correctAns)
        {
            //set the color of the selected option to green
            GameObject.FindGameObjectsWithTag("Selected")[0].GetComponent<Image>().color = new Color(0.5f, 1.0f, 0.5f, 1.0f);

            //set the outcome as "Correct Answer"
            canvasGO.GetComponent<SetQuestion>().Outcome.GetComponent<TextMeshProUGUI>().text = "Correct Answer!!";
        }
        else // if the selected option is incorrect
        {
            //set the color of the selected option to red
            GameObject.FindGameObjectsWithTag("Selected")[0].GetComponent<Image>().color = new Color(1.0f, 0.5f, 0.5f, 1.0f);

            //find the option with correct answer and set it to green
            for (int i = 0; i < 4; i++)
            {
                if (canvasGO.GetComponent<SetQuestion>().Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == canvasGO.GetComponent<SetQuestion>().mcq.correctAns)
                    canvasGO.GetComponent<SetQuestion>().Options.transform.GetChild(i).GetComponent<Image>().color = new Color(0.5f, 1.0f, 0.5f, 1.0f);
            }

            //set the outcome as "Wrong Answer"
            canvasGO.GetComponent<SetQuestion>().Outcome.GetComponent<TextMeshProUGUI>().text = "Wrong Answer!!";
        }

        //disable the 'Submit' button
        submitButton.interactable = false;

        //enable the 'Exit' button
        canvasGO.GetComponent<SetQuestion>().Exit.GetComponent<Button>().interactable = true;

    }
}
