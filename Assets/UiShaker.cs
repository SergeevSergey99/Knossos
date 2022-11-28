using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiShaker : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Shake());
    }
    
    private IEnumerator Shake()
    {
        var originalPosition = transform.position;
        var shakeDuration = 0.5f;
        var shakeMagnitude = 0.5f;
        var shakeSpeed = 10f;
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
    void Update()
    {
        
    }
}
