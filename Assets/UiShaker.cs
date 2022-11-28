using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiShaker : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.5f;
    public float shakeSpeed = 1.0f;
    
    private void Start()
    {
        
    }
    
    private IEnumerator Shake()
    {
        var originalPosition = transform.position;
        var shakeTime = 0f;
        while (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            var x = Mathf.Sin(shakeTime * shakeSpeed) * shakeMagnitude;
            var y = Mathf.Sin(shakeTime * shakeSpeed) * shakeMagnitude;
            transform.position = originalPosition + new Vector3(x, y, 0);
            yield return null;
        }
        
        transform.position = originalPosition;
    }
    
    public void StartShake()
    {
        StartCoroutine(Shake());
    }
}
