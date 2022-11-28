using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RotatableTiles : MonoBehaviour
{
    public float startX, startY;
    public int sizeX, sizeY;

    public int type;
    private Node[,] _array = null;
    private MAZE _maze = null;

    public void Init(MAZE maze)
    {
        this._maze = maze;
        _array = new Node[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                var localPosition = transform.parent.localPosition;
                var X = (int) Mathf.Round(localPosition.x + startX + j - maze.baseTilesList.startX);
                var Y = (int) Mathf.Round(localPosition.y + startY + i - maze.baseTilesList.startY);
                
                if (maze.Maze[X, Y] == null)
                {
                    var node = Instantiate(maze.nodePrefab, transform);
                    node.transform.localPosition = new Vector3(
                        0.5f + startX + j,
                        0.5f + startY + i,
                        transform.position.z
                    );


                    node.GetComponent<Node>().x = X;
                    node.GetComponent<Node>().y = Y;
                    maze.Maze[X, Y] = node.GetComponent<Node>();
                    _array[j, i] = node.GetComponent<Node>();
                    RaycastHit2D hit = Physics2D.Raycast(_array[j, i].transform.position, -Vector3.forward);
                    _array[j, i].isWall = hit.collider != null;
                    if(_array[j, i].isWall) _array[j, i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    public void RotateAllOfSameType90()
    {
        _maze.RotateAllOfType90(type);
    }
    public void RotateAllOfSameType_90()
    {
        _maze.RotateAllOfType_90(type);
    }

    public void childAddLayers(int i)
    {
        foreach (Transform node in transform)
        {
            foreach (Transform OBJ in node.transform)
            {
                if (OBJ.GetComponent<SpriteRenderer>() != null)
                {
                    OBJ.GetComponent<SpriteRenderer>().sortingOrder += i;
                }

                foreach (Transform VAR in OBJ)
                {
                    if (VAR.GetComponent<SpriteRenderer>() != null)
                    {
                        VAR.GetComponent<SpriteRenderer>().sortingOrder += i;
                    }
                }
            }
        }
    }
    

    public struct XYPair
    {
        public int x;
        public int y;
    }

    public void Rotate90()
    {
        transform.parent.Rotate(Vector3.forward * 90);
        transform.eulerAngles = new Vector3(0, 0, -90);
        XYPair[,] rotatedArray = new XYPair[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                rotatedArray[j, i].x = _array[sizeY-1-i, j].x;
                rotatedArray[j, i].y = _array[sizeY-1-i, j].y;
            }
        }
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                _array[i, j].x = rotatedArray[i, j].x;
                _array[i, j].y = rotatedArray[i, j].y;
                
                _maze.Maze[_array[i, j].x, _array[i, j].y] = _array[i, j];
            }
        }
        _maze.StopFixing();
        childAddLayers(11);
    }
    public void Rotate_90()
    {
        transform.parent.Rotate(Vector3.forward * -90);
        transform.eulerAngles = new Vector3(0, 0, 90);
        XYPair[,] rotatedArray = new XYPair[sizeX, sizeY];
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                rotatedArray[j, i].x = _array[i, sizeY-1-j].x;
                rotatedArray[j, i].y = _array[i, sizeY-1-j].y;
            }
        }
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                _array[i, j].x = rotatedArray[i, j].x;
                _array[i, j].y = rotatedArray[i, j].y;
                
                _maze.Maze[_array[i, j].x, _array[i, j].y] = _array[i, j];
            }
        }
        _maze.StopFixing();
        childAddLayers(11);
    }
}