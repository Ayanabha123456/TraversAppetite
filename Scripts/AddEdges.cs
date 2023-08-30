using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* class to add random edges to a graph of ingredients using Kruskal's algorithm */
//The code has been referred from this: https://www.geeksforgeeks.org/kruskals-minimum-spanning-tree-algorithm-greedy-algo-2/
public class AddEdges
{
    private List<string> keys; //list of nodes/ingredients in the graph
    private Dictionary<string, string> parent = new Dictionary<string, string>();
    private Dictionary<string, int> rank = new Dictionary<string, int>();

    /* class to represent a graph edge */
    class Edge : IComparer<Edge>
    {
        public string src, dest;
        public int weight;

        public int Compare(Edge e1, Edge e2)
        {
            return e1.weight - e2.weight;
        }
    }
    public AddEdges(List<string> keys)
    {
        this.keys = keys; //list of ingredients spawned in the scene

        //initialize the parent data structure - indicates the parent of each node in a particular component. Initially every node is their own parent
        foreach (string node in keys)
        {
            parent.Add(node, node);
        }
        //initialize the rank data structure - indicates approximately the height of the component starting from the parent. Initially it is zero
        foreach (string node in keys)
        {
            rank.Add(node, 0);
        }
    }
    /* function to recursively find the parent of a node in a component using path compression */
    string FindParent(string node)
    {
        if (parent[node] == node)
        {
            return node;
        }

        parent[node] = FindParent(parent[node]);

        return parent[node];
    }
    /* function to combine two separate components */
    void UnionSet(string u, string v)
    {
        string u_root = FindParent(u);
        string v_root = FindParent(v);

        //idea is to attach a component of smaller rank under a component with higher rank.
        if (rank[u_root] < rank[v_root])
        {
            parent[u_root] = v_root;
        }
        else if (rank[u_root] > rank[v_root])
        {
            parent[v_root] = u_root;
        }
        else
        {
            //If the ranks are equal, they can be attached in any manner and this increments the rank of the node which serves as the parent
            parent[v_root] = u_root;
            rank[u_root] = rank[u_root] + 1;
        }
    }
    /* We want to create a graph of ingredients that is connected with no cycles.  
     Kruskal's algorithm comes to mind but we don't have an initial graph that has weighted edges.
    So we find all possible edges for n ingredients assigning each of them a random weight.
    Then we run the normal Kruskal's algorithm that takes n-1 edges from all of these edges
    and provides us with a connected, uncyclic graph */
    public List<List<string>> Kruskal()
    {
        List<Edge> edges = new List<Edge>();

        /* create n(n-1)/2 edges for n ingredients with edge weight being a random value */
        for (int i = 0; i < keys.Count - 1; i++)
        {
            for (int j = i + 1; j < keys.Count; j++)
            {
                Edge edge = new Edge();
                edge.src = keys[i];
                edge.dest = keys[j];
                edge.weight = Random.Range(1, 20);

                edges.Add(edge);
            }
        }

        /* sort the edges in ascending order of weights */
        Edge e = new Edge();
        edges.Sort(e);

        List<List<string>> mst = new List<List<string>>();

        for (int i = 0; i < edges.Count; i++)
        {
            //get the parent of the source and destination for a particular edge
            string u = FindParent(edges[i].src);
            string v = FindParent(edges[i].dest);

            if (u != v) //if they are unequal, this means they do not form a cycle, hence add the edges to minimum spanning tree and combine the two components
            {
                List<string> ll = new List<string>();
                ll.Add(u); ll.Add(v);
                mst.Add(ll);
                UnionSet(u, v);
            }
        }

        return mst;
    }
}
