using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* class to find the BFS of a graph of ingredients */
//The code has been referred from this : https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/
public class BFS
{
    private Dictionary<string, List<string>> adj;

    public BFS(Dictionary<string, List<string>> adj)
    {
        this.adj = adj; //adjacency list of the graph of ingredients
    }

    //class to represent the ingredient and its level in BFS
    private class Vertex
    {
        public string name { get; }
        public int level { get; }

        public Vertex(string name, int level)
        {
            this.name = name;
            this.level = level;
        }
    }

    public Dictionary<string, int> BfsWithLevels(string source)
    {
        HashSet<string> visited = new HashSet<string>(); //data structure for visited ingredients in BFS
        Queue<Vertex> q = new Queue<Vertex>(); //queue for BFS

        q.Enqueue(new Vertex(source, 0)); //add the source ingredient and its level
        visited.Add(source); //add the source ingredient to the visited data structure
        Dictionary<string, int> res = new Dictionary<string, int>();

        while (q.Count > 0) //while queue is non-empty
        {
            Vertex v = q.Dequeue(); //dequeue the front ingredient
            string currNode = v.name;
            int level = v.level;
            res.Add(currNode, level); //add the (ingredient,level) to the result

            //iterate through its neighbors
            foreach (string neighbor in adj[currNode])
            {
                if (!visited.Contains(neighbor)) //if neighbor is not visited
                {
                    q.Enqueue(new Vertex(neighbor, level + 1)); //add it to queue along with level +1
                    visited.Add(neighbor); //add it to visited data structure
                }
            }
        }

        return res;
    }
}
