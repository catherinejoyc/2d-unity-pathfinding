using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    public Transform goal;
    public Graph grid;

    // Start is called before the first frame update
    void Start()
    {
        
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

            foreach (Vector2 next in grid.Neighbors(current))
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
