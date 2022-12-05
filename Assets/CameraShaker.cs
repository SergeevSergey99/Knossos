using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private float minMagnitude = 0;
    [SerializeField] private float maxMagnitude = 1;

    public void SetDuration(float newDur)
    {
        duration = newDur;
    }
    public void SetMaxMagnitude(float newMagn)
    {
        maxMagnitude = newMagn;
    }
    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCor());
    }
    
    IEnumerator ShakeCameraCor()
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.localPosition;
    
        while (elapsed < duration)
        {
            var magnitude = 0f;
            if(elapsed < duration / 2)
                magnitude = Mathf.Lerp(minMagnitude, maxMagnitude, elapsed / duration * 2);
            else
                magnitude = Mathf.Lerp(maxMagnitude, minMagnitude, (elapsed / duration - duration / duration /2) * 2);
            Vector2 cameraPostionDelta = Random.insideUnitCircle * magnitude;
    
            //Debug.Log(elapsed + " " + magnitude);
            transform.localPosition = startPosition + (Vector3) cameraPostionDelta;
   
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPosition;
    }
}
