using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* class that stores predefined post-game questions for the quiz (MCQScene) */
public class Questions : MonoBehaviour
{
    public Dictionary<string, MCQ[]> questionList = new Dictionary<string, MCQ[]>()
    {
        ["LIFO"] = new MCQ[] { new MCQ("Where to add/push elements into stack?", new string[] { "top","front","rear","anywhere"}, "top"),
                                new MCQ("Where to remove/pop elements from stack?", new string[]{"top","front","rear","anywhere" },"top"),
                                new MCQ("Can you remove elements from an empty stack?", new string[]{"Yes","No","Sometimes","Maybe" }, "No")},

        ["FIFO"] = new MCQ[] { new MCQ("Where to add/enqueue elements into queue?", new string[] { "top", "front", "rear", "anywhere" },"rear"),
                                new MCQ("Where to remove/dequeue elements from queue?", new string[] { "top", "front", "rear", "anywhere" },"front"),
                                new MCQ("When front and rear are at same position, the queue is...", new string[] { "full", "half-full", "empty", "half-empty" },"empty")},

        ["BFS"] = new MCQ[] { new MCQ("Breadth First Search starts from which node?", new string[] { "Any node", "Node with most edges", "Node with least edges", "Source node" }, "Source node"),
                                new MCQ("The source node is at which level?", new string[] { "0", "1", "2", "-1" },"0"),
                                new MCQ("A level 2 node is how many edges away from the source node?", new string[] { "0", "1", "2", "-1" },"2"),
                                new MCQ("Where to add/enqueue elements into queue?", new string[] { "top", "front", "rear", "anywhere" },"rear"),
                                new MCQ("Where to remove/dequeue elements from queue?", new string[] { "top", "front", "rear", "anywhere" },"front"),
                                new MCQ("When front and rear are at same position, the queue is...", new string[] { "full", "half-full", "empty", "half-empty" },"empty")},

        ["DFS"] = new MCQ[] { new MCQ("Depth First Search starts from which node?", new string[] { "Any node", "Node with most edges", "Node with least edges", "Source node" }, "Source node"),
                                new MCQ("What do you do if the node added to stack has no subsequent neighbors?", new string[] { "Pop/remove node from stack", "Backtrack to node that has subsequent nodes", "Start over from source node", "End the process" }, "Backtrack to node that has subsequent nodes"),
                                new MCQ("Backtracking involves what operation of stack?", new string[] { "Push/add nodes", "Check if stack is empty", "Pop/remove nodes", "Check if stack is full" },"Pop/remove nodes"),
                                new MCQ("For Depth First Search, except the source node, the node that is added to the stack must be......",new string[]{ "A subsequent neighbor of the previously added node","A node that is not a neighbor","Any node","A node 2 edges away from source node"},"A subsequent neighbor of the previously added node"),
                                new MCQ("Where to add/push elements into stack?", new string[] { "top","front","rear","anywhere"}, "top"),
                                new MCQ("Where to remove/pop elements from stack?", new string[]{"top","front","rear","anywhere" },"top"),
                                new MCQ("Can you remove elements from an empty stack?", new string[]{"Yes","No","Sometimes","Maybe" }, "No")}
    };
}
