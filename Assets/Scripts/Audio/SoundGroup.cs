using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Purpose: Define a sound object with properties for the AudioManager to use
public class SoundGroup
{
    public string name;

    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume;

    [Range(0f, 1f)]
    public float spacialBlend;

    [Range(0f, 1f)]
    public float spread;

    public string loopingClip;

    public AudioRolloffMode rolloffMode;
    public int maxDistance = 20;

    [Space]
    public Sound[] sounds;
}
