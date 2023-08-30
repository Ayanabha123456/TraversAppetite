using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* a class that defines every ingredient in the graph */
public class Node : MonoBehaviour
{
    public string nodeName; //name of the ingredient
    public Vector2 position; //it original position in the graph
    public List<Node> neighbors = new List<Node>(); //its neighbors
}
