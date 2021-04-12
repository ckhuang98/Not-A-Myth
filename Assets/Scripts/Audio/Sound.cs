using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
// Purpose: Define a sound object with properties for the AudioManager to use
public class Sound
{

    public string name;

    public AudioClip clip;

    // public bool volumeOverride;

    // [Range(0f, 1f)]
    // public float volume;

    // public bool loopOverride;

    [HideInInspector]
    public int index;

    [HideInInspector]
    public double length;

    [HideInInspector]
    public AudioSource source;

}
