using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeCenter : MonoBehaviour
{
    
    private Node _currNode = null;
    public bool isGear;
    public UnityEvent onActiveEvent;
    void Start()
    {
        var maze = FindObjectOfType<MAZE>();
        int closestX = 0, closestY = 0;
        float min = float.MaxValue;
        for (int i = 0; i < maze.baseTilesList.sizeX; i++)
        {
            for (int j = 0; j < maze.baseTilesList.sizeY; j++)
            {
                if ((maze.Maze[i, j].transform.position - transform.position).magnitude < min)
                {
                    min = (maze.Maze[i, j].transform.position - transform.position).magnitude;
                    closestX = i;
                    closestY = j;
                }
            }
        }

        _currNode = maze.Maze[closestX, closestY];
        transform.SetParent(_currNode.transform);
        transform.localPosition = Vector3.zero;
        _currNode.SetCenter(this);
    }
}
