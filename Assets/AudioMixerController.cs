using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMusic(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
    public void SetEffects(float value)
    {
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);
    }
}
