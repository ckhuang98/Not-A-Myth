using UnityEngine.Audio;
using System;
using UnityEngine;

public class ObjectAudioManagerDEPRICATED : MonoBehaviour
{
    [TextArea(1, 5)]
    public string description;

    public AudioMixer soundMixer;

    public SoundGroup[] soundGroups;

    // Purpose: Initialize AudioManager
    // Create and define an AudioSource for each Sound in sounds
    void Awake()
    {
        foreach (SoundGroup sg in soundGroups)
        {
            for (int i = 0; i < sg.sounds.Length; i++)
            {
                Sound s = sg.sounds[i];

                s.source = gameObject.AddComponent<AudioSource>();
                s.source.playOnAwake = false;
                s.source.clip = s.clip;

                s.source.outputAudioMixerGroup = sg.group;

                s.source.volume = sg.volume;

                // s.source.spatialBlend = sg.spacialBlend;
                // s.source.spread = sg.spread;

                s.source.loop = sg.loopingClip == s.name;

                // s.source.rolloffMode = sg.rolloffMode;
                // s.source.maxDistance = sg.maxDistance;
            }
        }
    }

    public SoundGroup PlayGroup(string name)
    {
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

    public void EndLoop(string name)
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

                    nextSound.source.loop = false;

                    nextSound.source.PlayScheduled(AudioSettings.dspTime
                        + (currentSound.source.clip.samples
                        - currentSound.source.timeSamples)
                        / currentSound.source.clip.frequency);
                }
            }
        }
    }

    public Sound PlayRandomSoundInGroup(string name)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);
        if (name == "Death")
        {
            Debug.Log(name);
        }

        if (sg.sounds.Length < 1)
            return null;

        Sound randomSound = sg.sounds[UnityEngine.Random.Range(0, sg.sounds.Length)];

        Play(randomSound);

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

    public Sound Play(Sound sound)
    {
        if (sound == null)
        {
            return null;
        }

        sound.source.Play();

        return sound;
    }

    private Sound PlayScheduled(Sound sound, double delay)
    {
        return sound;
    }
}
