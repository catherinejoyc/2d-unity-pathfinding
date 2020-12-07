using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    public Transform goal;
    public GridManager grid;

    // Start is called before the first frame update
    void Start()
    {
        BreadthFirstSearch();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BreadthFirstSearch()
    {
        Vector2 startPos = this.transform.position;

        var frontier = new Queue<Vector2>();
        frontier.Enqueue(startPos);

        var cameFrom = new Dictionary<Vector2, Vector2>();
        //cameFrom[startPos] = null;

        //breadcrumbs and pointer
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            Vector2[] neighbours = grid.graph.Neighbors(current);

            foreach (Vector2 next in neighbours)
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        //path creation
        var pos = (Vector2)goal.position;
        Queue<Vector2> path = new Queue<Vector2>();
        while(pos != startPos)
        {
            path.Enqueue(pos);
            pos = cameFrom[pos];
        }

        path = new Queue<Vector2>(path.Reverse());
    }
}
