using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{
    public KeyCode code;
    public KeyCode altCode;

    private Button btn = null;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (btn.interactable)
        {
            if(Input.GetKeyDown(code) || Input.GetKeyDown(altCode)) btn.onClick.Invoke();
        }
    }
}
