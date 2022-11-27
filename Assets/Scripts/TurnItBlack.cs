using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnItBlack : MonoBehaviour
{
    public GameObject black;

    private void Awake()
    {
        black.SetActive(true);
    }
}
