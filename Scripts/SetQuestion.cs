using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* a class for setting the question in the MCQ Scene */
public class SetQuestion : MonoBehaviour
{
    //Game objects where different parts of the MCQ are rendered
    public GameObject Question; 
    public GameObject Options; 
    public GameObject Submit;
    public GameObject Exit;
    public GameObject Outcome;
    public Color originalColor;
    private Dictionary<string, MCQ[]> questionList; //the list of questions for the algorithm
    public MCQ mcq; //one of the questions from the list
    
    void Start()
    {
        //get the set of questions
        questionList = GetComponent<Questions>().questionList;

        //exiting from the 'Fridge' Scene
        if(ManagerScript.Instance.haveIngredients)
        {
            //select a random MCQ for LIFO algorithm
            MCQ[] questions = questionList["LIFO"];
            int questionID = Random.Range(0, questions.Length);
            mcq = questions[questionID];
        }
        else //exiting from the 'Stove' Scene
        {
            //select a random MCQ for either FIFO, BFS or DFS algorithm
            MCQ[] questions = questionList[ManagerScript.Instance.questType];
            ManagerScript.Instance.questType = "";
            int questionID = Random.Range(0, questions.Length);
            mcq = questions[questionID];
        }

        //set the question and the options in the UI
        Question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mcq.question;

        for(int i = 0; i < 4; i++)
        {
            Options.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mcq.options[i];
            originalColor = Options.transform.GetChild(i).GetComponent<Image>().color;
        }

        //empty the outcome text
        Outcome.GetComponent<TextMeshProUGUI>().text = "";

        //disable the 'Submit' and 'Exit' button
        Submit.GetComponent<Button>().interactable = false;
        Exit.GetComponent<Button>().interactable = false;
    }

}
