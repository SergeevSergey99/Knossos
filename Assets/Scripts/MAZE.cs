using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MAZE : MonoBehaviour
{   
    
    [FormerlySerializedAs("BaseTilesList")] public RotatableTiles baseTilesList;
    [FormerlySerializedAs("RotatableTilesList")] public List<RotatableTiles> rotatableTilesList;

    public  Node[,] Maze = null;
    private NodeCenter[] _nodeCenters;
    [FormerlySerializedAs("NodePrefab")] public GameObject nodePrefab;
    

    private TurnManager _tm = null;

    private void Awake()
    {
        _nodeCenters = FindObjectsOfType<NodeCenter>();
        Maze = new Node[baseTilesList.sizeX, baseTilesList.sizeY];

        _tm = FindObjectOfType<TurnManager>();
        
        rotatableTilesList.Clear();
        var rtl = FindObjectsOfType<RotatableTiles>();
        foreach (var rt in rtl)
        {
            if(rt != baseTilesList)
                rotatableTilesList.Add(rt);
        }

        for (int i = 0; i < rotatableTilesList.Count; i++)
            rotatableTilesList[i].Init(this);

        baseTilesList.Init(this);
    }

    private Coroutine _fixCorr;

    public void RotateAllOfType90(int i)
    {
        foreach (var rt in rotatableTilesList)
        {
            if (rt.type == i) rt.transform.parent.GetComponent<Animator>().Play("Rotate90");
        }

        _fixCorr = StartCoroutine(Fixing());
    }

    public void RotateAllOfType_90(int i)
    {
        foreach (var rt in rotatableTilesList)
        {
            if (rt.type == i) rt.transform.parent.GetComponent<Animator>().Play("Rotate_90");
        }

        _fixCorr = StartCoroutine(Fixing());
    }

    public void StopFixing()
    {
        if (_fixCorr != null) StopCoroutine(_fixCorr);
        _fixCorr = null;
    }

    IEnumerator Fixing()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            FixRotation();
        }
    }

    public void FixRotation()
    {
        _tm.player.transform.eulerAngles = Vector3.zero;
        _tm.player.AnimStop();
        foreach (var nodeCenter in _nodeCenters)
        {
            nodeCenter.transform.eulerAngles = Vector3.zero;
        }

        foreach (var chel in _tm.cheliks)
        {
            chel.character.transform.eulerAngles = Vector3.zero;
        }
    }
}