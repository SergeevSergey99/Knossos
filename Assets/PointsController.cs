using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public PointType pointType;
    public List<Sprite> states;

    private MinotaurController player;
    private void Start()
    {
        player = FindObjectOfType<MinotaurController>();
    }

    public enum PointType
    {
        OD,
        OG
    }
    public void SetPoints(int i)
    {
        if (pointType == PointType.OD)
        {
            var val = i * 1f / player.MinotaurOD * states.Count;
        }
        else
        {
            
        }
    }
}
