using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class ObjectSound
{
    public string name;

    public AudioClip clip;

    [HideInInspector]
    public int index;

    [HideInInspector]
    public double length;

    [HideInInspector]
    public AudioSource source;
}
