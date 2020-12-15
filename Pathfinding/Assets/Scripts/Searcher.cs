using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    public Transform goal;
    public GridManager grid;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Queue<Vector2> path = BreadthFirstSearch();
        StartCoroutine(Walk(path));
    }

    // Update is called once per frame
    void Update()
    {
    }

    Queue<Vector2> BreadthFirstSearch()
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

            if (current == (Vector2)goal.position) //early exit
            {
                break;
            }

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

        return path;
    }



    IEnumerator Walk(Queue<Vector2> path)
    {
        while (path.Count > 0)
        {
            Vector2 next = new Vector2();
            next = path.Dequeue();

            while ((Vector2)this.transform.position != next)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, next, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

public class WalkCommand
{
    Vector2 goal;
    public WalkCommand(Vector2 pos)
    {
        goal = pos;
    }

    bool Execute(Transform motor)
    {
        while ((Vector2)motor.position != goal)
        {
            motor.Translate(goal);
        }
        return true;
    }
}
