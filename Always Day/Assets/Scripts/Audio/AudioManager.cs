﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            //s.source.bypassEffects = s.bypassEffects;
        }
    }

    private void Update()
    {
        //setPitchEffects();
    }

    // Changes the pitch of the audio clip according to Time.timeScale, clamped from 0 to 1.  
    //private void setPitchEffects()
    //{
    //    foreach (Sound s in sounds)
    //        if (!s.source.bypassEffects)
    //            s.source.pitch = Mathf.Clamp(Time.timeScale, 0.0f, 1.0f);  
    //}

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null)
            s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null)
            s.source.Stop();
    }
}