using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCharacter : MonoBehaviour
{
    private MAZE maze = null;

    private Node currNode = null;

    // Start is called before the first frame update
    void Start()
    {
        maze = FindObjectOfType<MAZE>();
        int closestX = 0, closestY = 0;
        float min = float.MaxValue;
        for (int i = 0; i < maze.BaseTilesList.sizeX; i++)
        {
            for (int j = 0; j < maze.BaseTilesList.sizeY; j++)
            {
                if ((maze.maze[i, j].transform.position - transform.position).magnitude < min)
                {
                    min = (maze.maze[i, j].transform.position - transform.position).magnitude;
                    closestX = i;
                    closestY = j;
                }
            }
        }

        currNode = maze.maze[closestX, closestY];
        transform.SetParent(currNode.transform);
        transform.localPosition = Vector3.zero;
    }

    public enum direction
    {
        left,
        right,
        up,
        down,
        none
    }

    public bool HaveCharacters(Node node)
    {
        foreach (Transform VARIABLE in node.transform)
        {
            if (VARIABLE.GetComponent<MazeCharacter>() != null)
                return true;
        }

        return false;
    }

    public List<direction> GetPossible(bool countCheliks = true)
    {
        List<direction> directions = new List<direction>();
        if (currNode.X > 0 && !maze.maze[currNode.X - 1, currNode.Y].isWall
                           && (!countCheliks || !HaveCharacters(maze.maze[currNode.X - 1, currNode.Y]))) 
            directions.Add(direction.left);
        
        if (currNode.X < maze.BaseTilesList.sizeX - 1 && !maze.maze[currNode.X + 1, currNode.Y].isWall
                                                      && (!countCheliks || !HaveCharacters(maze.maze[currNode.X + 1, currNode.Y])))
            directions.Add(direction.right);
        
        if (currNode.Y > 0 && !maze.maze[currNode.X, currNode.Y - 1].isWall
                           && (!countCheliks || !HaveCharacters(maze.maze[currNode.X, currNode.Y - 1]))) 
            directions.Add(direction.down);
        
        if (currNode.Y < maze.BaseTilesList.sizeY - 1 && !maze.maze[currNode.X, currNode.Y + 1].isWall
                                                      && (!countCheliks || !HaveCharacters(maze.maze[currNode.X, currNode.Y + 1])))
            directions.Add(direction.up);
        return directions;
    }
    public direction GetRandomPossible()
    {
        var directions = GetPossible();

        if (directions.Count == 0) return direction.none;
        return directions[Random.Range(0, directions.Count)];
    }

    public void MoveToRandomPossibleDir()
    {
        var dir = GetRandomPossible();
        MoveTo(dir);
    }
    public void MoveTo(direction dir)
    {
        switch (dir)
        {
            case direction.left:
                currNode = maze.maze[currNode.X - 1, currNode.Y];
                GetComponent<SpriteRenderer>().flipX = false;
                break;
            case direction.right:
                currNode = maze.maze[currNode.X + 1, currNode.Y];
                GetComponent<SpriteRenderer>().flipX = true;
                break;
            case direction.up:
                currNode = maze.maze[currNode.X, currNode.Y + 1];
                break;
            case direction.down:
                currNode = maze.maze[currNode.X, currNode.Y - 1];
                break;
            default:
                return;
        }

        transform.SetParent(currNode.transform);
        StartCoroutine(MoveToZero());
    }

    IEnumerator MoveToZero()
    {
        float t = 0.01f;
        while (transform.localPosition.magnitude > 0.05)
        {
            yield return new WaitForSeconds(t);
            transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, t * 5);
        }

        transform.localPosition = Vector3.zero;
    }
}