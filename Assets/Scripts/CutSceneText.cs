using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CutSceneText : MonoBehaviour
{
     public Animator animator;
     
    private void Awake()
    {
        Debug.Log("Zdarova");
        animator = GetComponent<Animator>();
    }

    public void Appear()
    {
        animator.Play("Appear");
    }
    public void Disappear()
    {
        animator.Play("Disappear");
    }
}
