using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip Clip1;
    public AudioClip Clip2;
    public AudioClip Clip3;

    AudioSource AS = null;
    bool secondClip = false;
    
    private void Awake()
    {
        AS = GetComponent<AudioSource>();
    }

    public void Play()
    {
        RandomPitch(0.1f);
        if (secondClip)
        {
            AS.clip = Clip2;
            AS.Play();
            secondClip = false;
        }
        else
        {
            AS.clip = Clip1;
            AS.Play();
            secondClip = true;
        }
    }

    public void PlayFirst()
    {
        AS.clip = Clip1;
        AS.Play();
    }
    
    public void PlaySecond()
    {
        AS.clip = Clip2;
        AS.Play();
    }
    
    public void PlayThird()
    {
        AS.clip = Clip3;
        AS.Play();
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
