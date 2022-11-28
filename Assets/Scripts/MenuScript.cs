using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private void Awake()
    {
        TurnItBlack.OnTurnBlack += TurnItBlack_OnTurnBlack;
    }

    

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ReLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlack();
    }
    
    private void TurnItBlack_OnTurnBlack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    
    public void DeActiveSelf()
    {
        gameObject.SetActive(false);
    }
}
