using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Node : MonoBehaviour
{
    [FormerlySerializedAs("X")] public int x;
    [FormerlySerializedAs("Y")] public int y;

    public bool isWall;

    public Transform character;
    [HideInInspector]public bool hasCharacter;
    
    [HideInInspector]public NodeCenter center;
    [HideInInspector]public bool hasCenter = false;
    
    public void SetCharacter(Transform character)
    {
        this.character = character;
        hasCharacter = true;
    }
    
    public void SetCenter(NodeCenter center)
    {
        this.center = center;
        hasCenter = true;
    }
}
