using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel(string name)
    {
        FindObjectOfType<RotateCounter>().counter = -1;
        SceneManager.LoadScene(name);
    }
    
    public void LoadLevelBlack(string name)
    {
        FindObjectOfType<RotateCounter>().counter = -1;
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlackAndLoadByIndex(name);
    }
    
    public void LoadLevel(int index)
    {
        //nextSceneToLoad = index;
        FindObjectOfType<RotateCounter>().counter = -1;
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlackAndLoadByIndex(index);
        
    }
    public void ReLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        //nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (FindObjectOfType<RotateCounter>().counter == -1)
        {
            FindObjectOfType<RotateCounter>().counter = 0;
            SceneManager.LoadScene(0);
        }
        FindObjectOfType<RotateCounter>().counter = 0;
        var Black = FindObjectOfType<TurnItBlack>();
        Black.TurnBlackAndLoadByIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetAudio(AudioClip clip)
    {
        var AS =FindObjectOfType<DontDestroy>().GetComponent<AudioSource>();
        AS.Stop();
        AS.clip = clip;
        AS.Play();
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
