using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnItBlack : MonoBehaviour
{
    public GameObject black;
    public GameObject permanentBlack;
    public static Action OnTurnBlack;
    private void Awake()
    {
        black.SetActive(true);
        permanentBlack.SetActive(false);
        black.GetComponent<Animator>().Play("DisappearZero");
    }
    
    public void TurnBlack()
    {
        black.GetComponent<Animator>().Play("AppearFromZero");
        //Invoke coroutine
        StartCoroutine(Wait(black.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
        
    }
    
    //Coroutine to wait for the animation to finish
    IEnumerator Wait(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        permanentBlack.SetActive(true);
        OnTurnBlack?.Invoke();
    }
    
}
