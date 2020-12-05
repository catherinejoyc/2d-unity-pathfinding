using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int height;
    public int width;
    public float nodeGap;
    public Graph graph;

    private void Awake()
    {
        graph = BuildGraph(this.transform.position, height, width, nodeGap);
    }

    //build from upper left to lower right corner
    Graph BuildGraph(Vector2 startPos, int h, int w, float nGap)
    {
        Graph _graph = new Graph();
        _graph.edges = new Dictionary<Vector2, Vector2[]>();

        //fill graph
        for (int yStatus = 0; yStatus < h; ++yStatus)
        {
            for (int xStatus = 0; xStatus < w; ++xStatus)
            {
                //set position
                float posY = startPos.y - (yStatus * nGap);
                float posX = startPos.x + (xStatus * nGap);

                Vector2 pos = new Vector2(posX, posY);

                //set neighbors
                Vector2[] posNeighbors = new Vector2[4] //right, down, left, up
                {
                    new Vector2(pos.x + nGap, pos.y),
                    new Vector2(pos.x, pos.y - nGap),
                    new Vector2(pos.x - nGap, pos.y),
                    new Vector2(pos.x, pos.y + nGap)
                };

                _graph.edges.Add(pos, posNeighbors);
            }
        }

        return _graph;
    }

    #region visuals
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        try
        {
            for (int yStatus = 0; yStatus < height; ++yStatus)
            {
                for (int xStatus = 0; xStatus < width; ++xStatus)
                {
                    //set position
                    float posY = this.transform.position.y - (yStatus * nodeGap);
                    float posX = this.transform.position.x + (xStatus * nodeGap);

                    Vector2 pos = new Vector2(posX, posY);

                    Gizmos.DrawWireSphere(pos, 0.1f);
                }
            }
            Gizmos.color = Color.red;
            try
            {
                foreach (Vector2 pos in graph.edges.Keys)
                {
                    Gizmos.DrawWireSphere(pos, 0.2f);
                }
            }
            catch { }
        }
        catch
        {
            Debug.LogError("Grid has no values (Height, Width, Node Gap)!");
        }
    }
    #endregion
}

public class Graph
{
    public Dictionary<Vector2, Vector2[]> edges = new Dictionary<Vector2, Vector2[]>();

    //return neighbors of a position
    public Vector2[] Neighbors(Vector2 pos)
    {
        return edges[pos];
    }
}
