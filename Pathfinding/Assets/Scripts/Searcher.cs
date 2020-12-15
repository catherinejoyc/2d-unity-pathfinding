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
        //point startPos to its nearest node in grid
        Vector2 startPos = this.transform.position;
        startPos = GetNearestNode(startPos);

        var frontier = new Queue<Vector2>();
        frontier.Enqueue(startPos);

        var cameFrom = new Dictionary<Vector2, Vector2>();

        //point goal position to its nearest node in grid
        Vector2 pathGoal = GetNearestNode(goal.position);

        //breadcrumbs and pointer
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == pathGoal) //early exit
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
        var pos = pathGoal;
        Queue<Vector2> path = new Queue<Vector2>();

        while(pos != startPos)
        {
            path.Enqueue(pos);

            pos = cameFrom[pos];
        }

        path = new Queue<Vector2>(path.Reverse());

        return path;
    }

    Queue<Vector2> AStarSearch()
    {
        //point startPos to its nearest node in grid
        Vector2 startPos = this.transform.position;
        startPos = GetNearestNode(startPos);

        var frontier = new Queue<Vector2>();
        frontier.Enqueue(startPos);

        var cameFrom = new Dictionary<Vector2, Vector2>();

        //point goal position to its nearest node in grid
        Vector2 pathGoal = GetNearestNode(goal.position);

        //breadcrumbs and pointer
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == pathGoal) //early exit
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
        var pos = pathGoal;
        Queue<Vector2> path = new Queue<Vector2>();

        while (pos != startPos)
        {
            path.Enqueue(pos);

            pos = cameFrom[pos];
        }

        path = new Queue<Vector2>(path.Reverse());

        return path;
    }

    Vector2 GetNearestNode(Vector2 currentPos)
    {
        var nodesInGrid = grid.graph.edges.Keys.ToArray<Vector2>();

        Vector2 nearestNode = nodesInGrid[0];
        float distance = Vector2.Distance(nodesInGrid[0], currentPos);

        for (int i = 0; i < nodesInGrid.Length; ++i)
        {
            //check distance
            if (Vector2.Distance(nodesInGrid[i], currentPos) < distance)
            {
                distance = Vector2.Distance(nodesInGrid[i], currentPos);
                nearestNode = nodesInGrid[i];
            }
        }

        return nearestNode;
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

    float Heuristic(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
