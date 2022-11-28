using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    private int _currentIndex = 0;
    public GameObject[] textObjects;
    private CutSceneText[] _textScripts;

    private void Awake()
    {
        _textScripts = new CutSceneText[textObjects.Length];
        for (int i = 0; i < textObjects.Length; i++)
        {
            _textScripts[i] = textObjects[i].GetComponent<CutSceneText>();
            textObjects[i].SetActive(false);

        }
    }

    private void Start()
    {
        ShowNext();
    }

    public void ShowNext()
    {
        
        if (_currentIndex < textObjects.Length)
        {
            if (_currentIndex > 0)
            {
                _textScripts[_currentIndex - 1].Disappear();
            }
            textObjects[_currentIndex].SetActive(true);
            _textScripts[_currentIndex].Appear();
            _currentIndex++;
        }
        else
        {
            _textScripts[_currentIndex-1].Disappear();
            if (SceneManager.GetActiveScene().buildIndex+1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
            
          
        }
        

    }
}