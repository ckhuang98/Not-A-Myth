using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Purpose: Define a sound object with properties for the AudioManager to use
public class Sound
{
    public string name;
    public string followingSound;

    public AudioClip clip;
    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
