using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
{
    [TextArea(maxLines: 10, minLines: 4)] [SerializeField]
    private string description = @"!!!DO NOT OVERRIDE THIS PREFAB!!!

    The AudioManager in a scene is for controlling universal sounds like music and menu noises.
    (Not for specific instance sounds like the player or giants. They have their own individual audio managers call ObjectAudioManager)

    Properties
    Sound Mixer is the mixer that controls the different audio levels

    Sound Groups is a list of individual sound collections with different adjustable volume and other properties.
    Each sound group contains another list of sounds for that group.This is where individual sound clips go.Each must be tied to a channel on the sound mixer.
    For example, you can have a sound group called Overworld Music which contains 3 sounds: the intro, the loop, and the outro.You can also specify which sound in the group you want to loop
    ";

    public AudioMixer soundMixer;
    public string playGroupOnStart;

    public SoundGroup[] soundGroups;

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
                s.source.playOnAwake = false;
                s.source.clip = s.clip;

                s.source.ignoreListenerPause = sg.ignorePause;
                s.source.outputAudioMixerGroup = sg.group;

                s.source.volume = sg.volume;

                // s.source.loop = s.loop;

                // s.source.spatialBlend = sg.spacialBlend;
                // s.source.spread = sg.spread;

                s.source.loop = sg.loopingClip == s.name;
            }
        }
    }

    void Start()
    {
        PlayGroup(playGroupOnStart);
    }

    void Update()
    {
        // Test the fade in and fade out of the music in the overworld group
        if (Input.GetKeyDown(KeyCode.P))
        {
            float vol = fadeToggle ? 0.0f : 1.0f;
            StartCoroutine(FadeMixerGroup.StartFade(soundMixer, "volumeOverworld", 2.0f, vol));
            fadeToggle = !fadeToggle;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            EndLoop("Overworld Music");
        }
    }

    //Purpose: Begin playing a sound group with this name
    //Will play all the sounds in a sound group in a row
    //If there is a sound in the group set to loop, it will begin to loop and will NOT play the following sounds
    public SoundGroup PlayGroup(string name)
    {
        Debug.Log("Playing Sound Group: " + name);
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);

        if (sg == null || sg.sounds.Length == 0)
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

    //Purpose: If there is a looping track playing in this sound group, turn off looping and let it play out
    //If there is a following sound in the soundgroup, it will play once the loop has finished its last cycle
    //TODO: currently only works if there is a looping track playing
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

    //Purpose: Play a random sound in a sound group
    public Sound PlayRandomSoundInGroup(string name)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.name == name);

        if (sg.sounds.Length < 1)
            return null;

        Sound randomSound = sg.sounds[UnityEngine.Random.Range(0, sg.sounds.Length)];

        Play(randomSound);

        return randomSound;
    }

    //Purpsose: Play a specific sound in a sound group
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

    //Purpose: Play a Sound
    public Sound Play (Sound sound)
    {
        if (sound == null)
        {
            return null;
        }

        sound.source.Play();

        return sound;
    }

    //Purpose: Schedule a Sound to be played. Currently not in use
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
