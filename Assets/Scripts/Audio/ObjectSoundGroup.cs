﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Purpose: Define a sound object with properties for the AudioManager to use
public class ObjectSoundGroup
{
    public string name;

    public AudioMixerGroup group;

    public bool ignorePause = false;

    [Range(0f, 1f)]
    public float volume;

    [Tooltip("Name of clip to loop once it starts playing")]
    public string loopingClip;

    // [Range(0f, 1f)]
    // public float spacialBlend = 1.0f;
    public bool spacialAudio = true;

    [Range(0f, 360f)]
    public float spread = 150.0f;

    public AudioRolloffMode rolloffMode;
    public int minDistance = 3;
    public int maxDistance = 20;

    [Space]
    public Sound[] sounds;
}
