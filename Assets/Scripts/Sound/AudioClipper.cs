using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipper
{
    public AudioClip clip;
    public bool loop;
    public float volume;

    public AudioClipper(AudioClip clip, bool loop, float volume)
    {
        this.clip = clip;
        this.loop = loop;
        this.volume = volume;
    }
}
