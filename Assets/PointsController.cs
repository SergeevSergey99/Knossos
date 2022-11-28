using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public PointerType Type;
    public List<Sprite> states;

    
    private void Start()
    {
        throw new NotImplementedException();
    }

    public enum PointType
    {
        OD,
        OG
    }
    public void SetPoints(int i)
    {
        
    }
}
