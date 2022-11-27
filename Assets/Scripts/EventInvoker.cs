using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour
{
    public List<UnityEvent> invokeEvent;

    public void INVOKE(int i)
    {
        invokeEvent[i].Invoke();
    }
}
