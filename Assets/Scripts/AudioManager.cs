using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer musicMixer;

    public Sound[] sounds;

    // fade down (true) fade up (false). Used for testing at the moment
    private bool fadeToggle;

    // Purpose: Initialize AudioManager
    // Create and define an AudioSource for each Sound in sounds
    void Awake()
    {
        fadeToggle = true;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.group;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        //Start playing the Demo1 sound at start
        Play("Demo1");
    }

    void Update()
    {
        // Test the fade in and fade out of the music in the overworld group
        if (Input.GetKeyDown(KeyCode.P))
        {
            float vol = fadeToggle ? 0.0f : 1.0f;
            StartCoroutine(FadeMixerGroup.StartFade(musicMixer, "volumeOverworld", 2.0f, vol));
            fadeToggle = !fadeToggle;
        }
    }

    // Purpose: Play the sound from the given sound name
    // If the sound has a following sound, schedule that sound for playback
    public Sound Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return null;
        }
        s.source.Play();

        if (s.followingSound != null)
        {
            // double delay = (amount of clip samples) / (clip frequency) - This is the length of the clip in DSP time
            PlayScheduled(s.followingSound, (double)(s.source.clip.samples / s.source.clip.frequency));
        }

        return s;
    }

    // Purpose: Play the sound from the given name after a delay
    // delay is a double and the timing is based off the sample rate
    // Then length of an audio clip in DSP time is the amount of samples of the clip divided by the clip's frequency
    public Sound PlayScheduled(string name, double delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return null;
        }

        Debug.Log("Scheduling " + s.name);
        s.source.PlayScheduled(AudioSettings.dspTime + delay);

        return s;
    }
}
