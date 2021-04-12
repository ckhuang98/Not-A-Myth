using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Purpose: Define a sound object with properties for the AudioManager to use
public class SoundGroup
{
    public string name;

    public AudioMixerGroup group;

    public bool ignorePause = false;

    [Range(0f, 1f)]
    public float volume;

    public string loopingClip;

    [Space]
    public Sound[] sounds;
}
