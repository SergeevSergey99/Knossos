using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnItBlack : MonoBehaviour
{
    public GameObject black;
    public GameObject permanentBlack;
    //public static Action OnTurnBlack;
    private void Awake()
    {
        black.SetActive(true);
        permanentBlack.SetActive(false);
        black.GetComponent<Animator>().Play("DisappearZero");
    }
    
    public void TurnBlackAndLoadByIndex(int index)
    {
        black.GetComponent<Animator>().Play("AppearFromZero");
        //Invoke coroutine
        StartCoroutine(Wait(black.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, index));
    }
    public void TurnBlackAndLoadByIndex(string sceneName)
    {
        black.GetComponent<Animator>().Play("AppearFromZero");
        //Invoke coroutine
        StartCoroutine(Wait(black.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length, sceneName));
    }
    
    //Coroutine to wait for the animation to finish
    IEnumerator Wait(float time, int index)
    {
        yield return new WaitForSecondsRealtime(time);
        permanentBlack.SetActive(true);
        //OnTurnBlack?.Invoke();
        SceneManager.LoadScene(index);
    }
    //Coroutine to wait for the animation to finish
    IEnumerator Wait(float time, string sceneName)
    {
        yield return new WaitForSecondsRealtime(time);
        permanentBlack.SetActive(true);
        //OnTurnBlack?.Invoke();
        SceneManager.LoadScene(sceneName);
    }
    
}
