using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* class that stores predefined recipes */
public class Recipes : MonoBehaviour
{
    /* recipes for FIFO and BFS */
    public Dictionary<string, string[]> recipes = new Dictionary<string, string[]>()
    {
        ["Chicken curry"] = new string[]{ "oil", "onion","tomato","red pepper","potato","chicken","spice1","spice2","water","mint"},
        ["Steak with wine"] = new string[] {"meat","oil","spice1","spice2","garlic","butter","dill","lemon","salad","wine"},
        ["Bread & butter pudding"] = new string[] {"butter","eggs","milk","honey","nut","cherry","coconut","bread","ice cream","strawberry"}
    };
    /* predefined graph edges for BFS */
    public Dictionary<string, List<List<string>>> recipeEdges = new Dictionary<string, List<List<string>>>()
    {
        ["Chicken curry"] = new List<List<string>>() { new List<string>() {"oil","onion" }, new List<string>() { "onion", "tomato" }, new List<string>() { "onion", "red pepper" },
        new List<string>() {"tomato","potato" }, new List<string>() {"red pepper","potato" }, new List<string>() {"potato","chicken" }, new List<string>() {"chicken","spice1" }, new List<string>() {"chicken","spice2" },
        new List<string>() {"spice1","water" }, new List<string>() {"spice2","water" }, new List<string>() {"water","mint" }},

        ["Steak with wine"] = new List<List<string>>() { new List<string>() {"meat","oil" }, new List<string>() { "oil", "spice1" }, new List<string>() { "oil", "spice2" },
        new List<string>() { "spice1", "garlic" }, new List<string>() { "spice1", "butter" }, new List<string>() { "spice2", "garlic" }, new List<string>() { "spice2", "butter" }, new List<string>() {"garlic","dill" },
        new List<string>() {"butter","dill" }, new List<string>() {"dill","lemon" }, new List<string>() {"lemon","salad" }, new List<string>() {"salad","wine" }},

        ["Bread & butter pudding"] = new List<List<string>>() { new List<string>() {"butter","eggs" }, new List<string>() { "eggs", "milk" }, new List<string>() { "milk", "honey" },
        new List<string>() {"honey","nut" }, new List<string>() {"honey","cherry" }, new List<string>() {"honey","coconut" }, new List<string>() {"nut","bread" }, new List<string>() {"cherry","bread" },
        new List<string>() {"coconut","bread" }, new List<string>() {"bread","ice cream" }, new List<string>() {"ice cream","strawberry" }}
    };
}
