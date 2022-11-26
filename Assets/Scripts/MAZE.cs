using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAZE : MonoBehaviour
{
    public RotatableTiles BaseTilesList;
    public List<RotatableTiles> RotatableTilesList;

    public Node[,] maze = null;
    public GameObject NodePrefab;


    private void Start()
    {
        maze = new Node[BaseTilesList.sizeX, BaseTilesList.sizeY];

        for (int i = 0; i < RotatableTilesList.Count; i++)
        {
            RotatableTilesList[i].init(this);
        }

        BaseTilesList.init(this);
    }

    public void RotateAllOfType90(int i)
    {
        foreach (var rt in RotatableTilesList)
        {
            if (rt.type == i) rt.Rotate90();
        }
    }
}