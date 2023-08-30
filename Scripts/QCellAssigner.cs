using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* class that defines each queue cell */
public class QCellAssigner : MonoBehaviour
{
    public string cellID; //stores the identifier for the expected ingredient in FIFO or BFS
    //flags to check if the queue cell is front or rear
    public bool isFront = false;
    public bool isRear = false;
}
