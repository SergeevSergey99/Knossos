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


    private TurnManager tm = null;
    private void Awake()
    {
        maze = new Node[BaseTilesList.sizeX, BaseTilesList.sizeY];

        tm = FindObjectOfType<TurnManager>();
        
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
            if (rt.type == i) rt.GetComponent<Animator>().Play("Rotate90");
        }
    }
    public void RotateAllOfType_90(int i)
    {
        foreach (var rt in RotatableTilesList)
        {
            if (rt.type == i) rt.GetComponent<Animator>().Play("Rotate_90");
        }
    }

    public void fixRotation()
    {
        tm.player.transform.eulerAngles = Vector3.zero;
        foreach (var chel in tm.cheliks)
        {
            chel.transform.eulerAngles  = Vector3.zero;
        }
    }
}