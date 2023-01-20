using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioObject;
    public List<AudioClipper> soundQueue = new List<AudioClipper>();
    public List<AudioPlayer> soundsPlaying;
    public float volume = 0.5f;

    private void Update()
    {
        if (soundQueue == null)
            return;
        
        foreach (var clip in soundQueue.ToArray())
        {
            AudioPlayer source = Instantiate(audioObject).GetComponent<AudioPlayer>();
            source.audioSource.clip = clip.clip;
            source.audioSource.volume = clip.volume;
            source.soundsPlaying = soundsPlaying;
            source.loop = clip.loop;
            
            soundsPlaying.Add(source);
            soundQueue.Remove(clip);
        }
    }

    public AudioSource GetSound(AudioClip clip)
    {
        if (soundsPlaying == null)
            return new AudioSource();
        
        foreach (var source in soundsPlaying)
        {
            if (source.audioSource.clip == clip)
            {
                return source.audioSource;
            }
        }

        return new AudioSource();
    }

    public void AddSoundToQueue(AudioClip clip, bool loop, float volume)
    {
        soundQueue.Add(new AudioClipper(clip, loop, volume));
    }

    public void StopAllSounds()
    {
        if (soundsPlaying == null)
            return;
        
        foreach (var sound in soundsPlaying.ToArray())
        {
            sound.audioSource.Stop();
            Destroy(sound.gameObject);
        }
    }
}
