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

    private Coroutine fixCorr = null;

    public void RotateAllOfType90(int i)
    {
        foreach (var rt in RotatableTilesList)
        {
            if (rt.type == i) rt.transform.parent.GetComponent<Animator>().Play("Rotate90");
        }

        fixCorr = StartCoroutine(fixing());
    }

    public void RotateAllOfType_90(int i)
    {
        foreach (var rt in RotatableTilesList)
        {
            if (rt.type == i) rt.transform.parent.GetComponent<Animator>().Play("Rotate_90");
        }

        fixCorr = StartCoroutine(fixing());
    }

    public void StopFixing()
    {
        if (fixCorr != null) StopCoroutine(fixCorr);
        fixCorr = null;
    }

    IEnumerator fixing()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            fixRotation();
        }
    }

    public void fixRotation()
    {
        tm.player.transform.eulerAngles = Vector3.zero;
        tm.player.AnimStop();
        foreach (var VARIABLE in FindObjectsOfType<NodeCenter>())
        {
            VARIABLE.transform.eulerAngles = Vector3.zero;
        }

        foreach (var chel in tm.cheliks)
        {
            chel.transform.eulerAngles = Vector3.zero;
        }
    }
}