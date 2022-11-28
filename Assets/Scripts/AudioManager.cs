using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips;

    AudioSource AS = null;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    public void Play(int i)
    {
        if (i >= 0 && i < clips.Count)
        {
            AS.clip = clips[i];
            AS.Play();
        }
    }

    public void Stop()
    {
        AS.Stop();
    }
}
