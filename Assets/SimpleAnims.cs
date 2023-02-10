using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnims : MonoBehaviour
{
    public float animTime = 0.5f;
    public float delay = 0.5f;
    Vector3 startPosition;
    
    public void RememberStartPosition()
    {
        startPosition = transform.position;
    }
    public void InstantMoveX(float x)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y,
            transform.localPosition.z);
    }
    public void MoveX(float x)
    {
        LeanTween.moveLocalX(gameObject, transform.localPosition.x + x, animTime);
    }

    public void MoveToStartAfter()
    {
        var seq = LeanTween.sequence();
        seq.append(delay);
        seq.append(LeanTween.move(gameObject, startPosition, animTime));
        
    }
}
