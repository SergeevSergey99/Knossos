using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        var gos = FindObjectsOfType<DontDestroy>();
        foreach (var go in gos)
        {
            if(go != this) Destroy(go.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
