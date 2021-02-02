﻿using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer musicMixer;

    public SoundGroup[] soundGroups;

    // public Sound[] sounds;

    // fade down (true) fade up (false). Used for testing at the moment
    private bool fadeToggle;

    // Purpose: Initialize AudioManager
    // Create and define an AudioSource for each Sound in sounds
    void Awake()
    {
        fadeToggle = true;

        foreach (SoundGroup sg in soundGroups)
        {
            for (int i = 0; i < sg.sounds.Length; i++)
            {
                Sound s = sg.sounds[i];

                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.outputAudioMixerGroup = sg.group;

                s.source.volume = sg.volume;

                s.source.spatialBlend = sg.spacialBlend;
                s.source.spread = sg.spread;

                s.source.loop = sg.loopingClip == s.name;
            }
        }
    }

    void Start()
    {
        PlayGroup("Overworld Music");
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            EndLoop("Overworld Music");
        }
    }

    public SoundGroup PlayGroup(string name)
    {
        Debug.Log("Playing Sound Group: " + name);
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);

        if (sg == null)
        {
            return null;
        }

        sg.sounds[0].source.Play();

        if (sg.sounds.Length > 1)
        {
            double totalDelay = 0;

            for (int i = 1; i < sg.sounds.Length; i++)
            {
                Sound currentSound = sg.sounds[i];
                Sound previousSound = sg.sounds[i - 1];

                totalDelay += (previousSound.source.clip.samples / previousSound.source.clip.frequency);
                currentSound.source.PlayScheduled(AudioSettings.dspTime + totalDelay);

                if (currentSound.source.loop)
                {
                    break;
                }
            }
        }

        return sg;
    }

    public void EndLoop (string name)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);

        for (int i = 0; i < sg.sounds.Length; i++)
        {
            if (sg.sounds[i].source.isPlaying)
            {
                Sound currentSound = sg.sounds[i];
                currentSound.source.loop = false;

                if (i + 1 < sg.sounds.Length)
                {
                    Sound nextSound = sg.sounds[i + 1];

                    nextSound.source.PlayScheduled(AudioSettings.dspTime
                        + (currentSound.source.clip.samples
                        - currentSound.source.timeSamples)
                        / currentSound.source.clip.frequency);
                }
            }
        }
    }

    public Sound PlayRandomSoundInGroup(string name, Vector3 position = new Vector3())
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);

        if (sg.sounds.Length < 1)
            return null;

        Sound randomSound = sg.sounds[UnityEngine.Random.Range(0, sg.sounds.Length)];

        Play(randomSound, position);

        return randomSound;
    }

    public Sound PlaySoundInGroup(string groupName, string soundName)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == groupName);

        Sound s = Array.Find(sg.sounds, sound => sound.name == soundName);

        if (s != null)
        {
            Play(s);
        }

        return s;
    }

    public Sound Play (Sound sound)
    {
        if (sound == null)
        {
            return null;
        }

        sound.source.Play();

        return sound;
    }

    public Sound Play (Sound sound, Vector3 position)
    {
        if (sound == null)
            return null;

        // AudioSource.PlayClipAtPoint(sound.source.clip, position);

        return sound;
    }

    private Sound PlayScheduled(Sound sound, double delay)
    {
        return sound;
    }

    // Purpose: Play the sound from the given sound name
    // If the sound has a following sound, schedule that sound for playback
    //public Sound Play(string name)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    if (s == null)
    //    {
    //        Debug.LogWarning("Sound " + name + " not found");
    //        return null;
    //    }
    //    s.source.Play();

    //    if (s.followingSound != null)
    //    {
    //        // double delay = (amount of clip samples) / (clip frequency) - This is the length of the clip in DSP time
    //        PlayScheduled(s.followingSound, (double)(s.source.clip.samples / s.source.clip.frequency));
    //    }

    //    return s;
    //}

    // Purpose: Play the sound from the given name after a delay
    // delay is a double and the timing is based off the sample rate
    // Then length of an audio clip in DSP time is the amount of samples of the clip divided by the clip's frequency
    //public Sound PlayScheduled(string name, double delay)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    if (s == null)
    //    {
    //        Debug.LogWarning("Sound " + name + " not found");
    //        return null;
    //    }

    //    Debug.Log("Scheduling " + s.name);
    //    s.source.PlayScheduled(AudioSettings.dspTime + delay);

    //    return s;
    //}
}
