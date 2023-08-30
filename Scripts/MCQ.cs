using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* a class that defines each MCQ for the MCQScene */
public class MCQ
{
    public string question; //the question
    public string[] options; //the set of options
    public string correctAns; //the correct answer

    public MCQ(string question, string[] options, string correctAns)
    {
        this.question = question;
        this.options = options;
        this.correctAns = correctAns;
    }
}
