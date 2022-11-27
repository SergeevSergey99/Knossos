using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeCenter : MonoBehaviour
{
    private Node currNode = null;
    public bool isGear;
    public UnityEvent onActiveEvent;
    void Start()
    {
        var maze = FindObjectOfType<MAZE>();
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
}
