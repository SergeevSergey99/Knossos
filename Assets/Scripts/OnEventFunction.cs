using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEventFunction : MonoBehaviour
{
    public UnityEvent OnAwakeFunctions;
    public UnityEvent OnStartFunctions;
    public UnityEvent OnEnableFunctions;
    public UnityEvent OnDisableFunctions;
    public UnityEvent OnDestroyFunctions;
    private void Awake()
    {
        OnAwakeFunctions.Invoke();
    }

    void Start()
    {
        OnStartFunctions.Invoke();
    }

    private void OnEnable()
    {
        OnEnableFunctions.Invoke();
    }

    private void OnDisable()
    {
        OnDisableFunctions.Invoke();
    }

    private void OnDestroy()
    {
        OnDestroyFunctions.Invoke();
    }
}
