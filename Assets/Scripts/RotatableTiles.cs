using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableTiles : MonoBehaviour
{
    public float startX, startY;
    public int sizeX, sizeY;

    public int type;
    private Node[,] Array = null;
    private MAZE maze = null;

    public void init(MAZE maze)
    {
        this.maze = maze;
        Array = new Node[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                var X = (int) Mathf.Round(transform.localPosition.x + startX + j - maze.BaseTilesList.startX);
                var Y = (int) Mathf.Round(transform.localPosition.y + startY + i - maze.BaseTilesList.startY);
                
                if (maze.maze[X, Y] == null)
                {
                    var node = Instantiate(maze.NodePrefab, transform);
                    node.transform.localPosition = new Vector3(
                        0.5f + startX + j,
                        0.5f + startY + i,
                        transform.position.z
                    );


                    node.GetComponent<Node>().X = X;
                    node.GetComponent<Node>().Y = Y;
                    maze.maze[X, Y] = node.GetComponent<Node>();
                    Array[j, i] = node.GetComponent<Node>();
                    RaycastHit2D hit = Physics2D.Raycast(Array[j, i].transform.position, -Vector3.forward);
                    Array[j, i].isWall = hit.collider != null;
                    if(Array[j, i].isWall) Array[j, i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    public void RotateAllOfSameType90()
    {
        maze.RotateAllOfType90(type);
    }
    public void RotateAllOfSameType_90()
    {
        maze.RotateAllOfType_90(type);
    }

    public struct xyPair
    {
        public int x;
        public int y;
    }

    public void Rotate90()
    {
        transform.Rotate(Vector3.forward * 90);
        xyPair[,] rotatedArray = new xyPair[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                rotatedArray[j, i].x = Array[sizeY-1-i, j].X;
                rotatedArray[j, i].y = Array[sizeY-1-i, j].Y;
            }
        }
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Array[i, j].X = rotatedArray[i, j].x;
                Array[i, j].Y = rotatedArray[i, j].y;
                
                maze.maze[Array[i, j].X, Array[i, j].Y] = Array[i, j];
            }
        }
        maze.fixRotation();
    }
    public void Rotate_90()
    {
        transform.Rotate(Vector3.forward * -90);
        xyPair[,] rotatedArray = new xyPair[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                rotatedArray[j, i].x = Array[i, sizeY-1-j].X;
                rotatedArray[j, i].y = Array[i, sizeY-1-j].Y;
            }
        }
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Array[i, j].X = rotatedArray[i, j].x;
                Array[i, j].Y = rotatedArray[i, j].y;
                
                maze.maze[Array[i, j].X, Array[i, j].Y] = Array[i, j];
            }
        }
        maze.fixRotation();
    }
}