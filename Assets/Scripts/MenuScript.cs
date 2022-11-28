using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    
    int nextSceneToLoad;
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
    
    public void LoadLevel(int index)
    {
        nextSceneToLoad = index;
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlack();
        
    }
    public void ReLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlack();
    }
    
    private void TurnItBlack_OnTurnBlack()
    {
        SceneManager.LoadScene(nextSceneToLoad);

    }
    
    public void DeActiveSelf()
    {
        gameObject.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    
    public void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }
}
