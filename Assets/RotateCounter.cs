using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCounter : MonoBehaviour
{
    public int counter = 0;
    
    public void Increment()
    {
        if(counter >= 0) counter++;
    }
}
