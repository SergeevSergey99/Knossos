using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        AS.clip = null;
    }

    public void SetPitch(float pitch)
    {
        AS.pitch = pitch;
    }
    public void RandomPitch(float offset)
    {
        if (offset < 1) AS.pitch = Random.Range(1 - offset, 1 + offset);
    }
}
