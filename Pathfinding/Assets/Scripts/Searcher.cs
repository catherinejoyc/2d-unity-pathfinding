using System.Collections;
using System.Collections.Generic;
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
        Vector2 currentPos = this.transform.position;

        var frontier = new Queue<Vector2>();
        frontier.Enqueue(currentPos);

        var reached = new HashSet<Vector2>();
        reached.Add(currentPos);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(goal.position))
            {
                //walk
                foreach(Vector2 pos in reached)
                {
                    this.transform.Translate(pos);
                }
                break;
            }

            Vector2[] neighbours = grid.graph.Neighbors(current);

            foreach (Vector2 next in neighbours)
            {
                if (!reached.Contains(next))
                {
                    frontier.Enqueue(next);
                    reached.Add(next);
                }
            }
        }
    }
}
