using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudioManager : MonoBehaviour
{
    [TextArea(maxLines: 10, minLines: 4)]
    [SerializeField]
    private string description = @"The Object Audio Manager is for sounds emitted from objects in the scene (the player, enemies, environment, etc.)
    
    Properties:
    Audio Mixer: Should be set to SoundMixer
    Play Group On Start: The name of the group that will be played on Start
    Volume: Volume of the clips in the group. Can be overridden by the individual groups.
            Value 0 - 1. NOT THE SAME AS THE MIXER'S VOLUME.
    Spread: How the audio is balanced in stereo. 180 is perfectly balanced.
    AudioRolloffMode: How the audio fades as the source gets further away
    Min Distance: The minimum distance from the source for the sound to be at full loudness
    Max Distance: THe maximum distance from which the source can still be heard
    Sound Groups: Array of SoundGroups which hold the different Sounds
                  ex. the 'footsteps' SoundGroup can hold all the footstep Sounds";

    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroup;

    public bool spacialBlend = true;

    [Tooltip("Name of group to play on Start")]
    public string playGroupOnStart;

    [Range(0f,1f)]
    public float volume = 1f;

    [Range(0f, 360f)]
    public float spread = 150f;

    public AudioRolloffMode rolloffMode;

    public float minDistance = 3;
    public float maxDistance = 10;

    public bool isPlayerSfx = false;

    [Space]
    public ObjectSoundGroup[] soundGroups;

    // fade down (true) fade up (false). Used for Testing at the moment
    private bool test_fadeToggle;

    private void Awake()
    {
        test_fadeToggle = true;

        foreach (ObjectSoundGroup sg in soundGroups)
        {
            int index = 0;
            // foreach (Sound s in sg.sounds)
            // {
            //     s.index = index;                                    // set the index of the sound in the group

            //     s.source = gameObject.AddComponent<AudioSource>();  // add the actual audio source
            //     s.source.playOnAwake = false;                       // so clip won't immediately play
            //     s.source.clip = s.clip;                             // set the source's sound clip

            //     s.source.ignoreListenerPause = sg.ignorePause;      // if true, sound will continue to play when AudioListener is paused
            //     s.source.outputAudioMixerGroup = sg.group;          // set the sound group from the audio mixer

            //     s.source.volume = sg.volume;                        // set the volume 0 - 1. NOT THE SAME AS THE MIXER GROUP'S VOLUME

            //     s.source.loop = sg.loopingClip == s.name;           // set whether the clip loops
            //     s.length = s.source.clip.samples / s.source.clip.frequency;
            //     index++;
            //     if (s.name == "") s.name = index.ToString();

            //     s.source.spatialBlend = sg.spacialAudio ? 1.0f : 0.0f;
            // }

            foreach (Sound s in sg.sounds){
                s.index = index; // set the index of the sound in the group
                s.source = gameObject.AddComponent<AudioSource>(); // the actual audio source
                s.source.playOnAwake = false; // clip will not play immediately
                s.source.clip = s.clip;

                s.source.outputAudioMixerGroup = audioMixerGroup; // set the mixer group from the audio mixer

                s.source.rolloffMode = rolloffMode;

                s.source.volume = sg.overrideVolume ? sg.volume : volume; // set the volume 0 - 1. NOT THE SAME AS THE MIXER GROUP'S VOLUME

                s.source.loop = (sg.loopingClip == s.name && s.name != ""); // set whether the clip loops

                s.length = s.source.clip.samples / s.source.clip.frequency;

                s.source.spatialBlend = spacialBlend ? 1.0f: 0.0f;
                s.source.spread = spread;

                s.source.ignoreListenerPause = sg.ignorePause;

                s.source.minDistance = sg.overrideDistances ? sg.minDistance : minDistance;
                s.source.maxDistance = sg.overrideDistances ? sg.maxDistance : maxDistance;

                if(isPlayerSfx){
                    s.source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                }
                

                index++;
                if (s.name == "") s.name = index.ToString();
            }
        }
    }

    private void Start()
    {
        // playGroupOnStart(playGroupOnStart);
        PlayGroup(playGroupOnStart);
    }

    // Purpose: Find sound group with given name
    private ObjectSoundGroup FindSoundGroup(string name)
    {
        return Array.Find(soundGroups, soundGroup => soundGroup.name == name);
    }

    private Sound FindSoundInGroup(ObjectSoundGroup sg, string sName)
    {
        if (sg == null) return null;
        return Array.Find(sg.sounds, s => s.name == sName);
    }

    private Sound FindSoundInGroup(string sgName, string sName)
    {
        return FindSoundInGroup(FindSoundGroup(sgName), sName);
    }

    // Purpose: Get the sound at the given index of the sound group from the given name
    private Sound GetSoundAtIndex(string name, int index)
    {
        return FindSoundGroup(name)?.sounds[index]; // this notation is wack, but it works. Returns null if not found
    }

    // Purpose: get the currently playing sound in the group
    private Sound GetCurrentlyPlayingSound(string name)
    {
        return GetCurrentlyPlayingSound(FindSoundGroup(name));
    }
    private Sound GetCurrentlyPlayingSound(ObjectSoundGroup sg)
    {
        if (sg == null) return null; // if group is not found

        foreach (Sound s in sg.sounds)
        {
            if (s.source.isPlaying) return s;
        }

        return null; // if no sound in group is playing
    }

    // Purpose: get the delays for all songs in the group
    // Ex. The first delay will be the length of the first clip,
    // the second will be the length of both the first and second clip,
    // and the third will be the length of the first + the second + the third
    // and so on
    private List<double> GetDelays(string name)
    {
        return (GetDelays(FindSoundGroup(name)));
    }

    private List<double> GetDelays(ObjectSoundGroup sg)
    {
        List<double> delays = new List<double>();
        double totalDelay = 0;
        delays.Add(totalDelay);

        foreach (Sound s in sg.sounds)
        {
            totalDelay += s.length;
            delays.Add(totalDelay);
        }

        return delays;
    }

    // Purpose: if the sound group is playing, return an array of the times left for each song to play
    // Does not account for looping
    private List<double> GetDelaysFromPlaying(string name)
    {
        return GetDelaysFromPlaying(FindSoundGroup(name));
    }

    private List<double> GetDelaysFromPlaying(ObjectSoundGroup sg)
    {
        List<double> delayList = new List<double>();
        double totalDelay = 0;
        delayList.Add(totalDelay);

        // Get the time left for the currently playing clip
        Sound currentlyPlayingSound = GetCurrentlyPlayingSound(sg);
        if (currentlyPlayingSound == null) return null;

        totalDelay += (currentlyPlayingSound.source.clip.samples
            - currentlyPlayingSound.source.timeSamples)
            / currentlyPlayingSound.source.clip.frequency;

        delayList.Add(totalDelay);

        // add the lengths of the next songs that will play

        for (int i = currentlyPlayingSound.index + 1; i < sg.sounds.Length; i++)
        {
            Sound currentSound = sg.sounds[i];

            totalDelay += currentSound.source.clip.samples / currentSound.source.clip.frequency;

            delayList.Add(totalDelay);
        }

        return delayList;
    }

    public Sound Play(Sound s, bool alterPitch = false)
    {
        if (s == null) return null;

        if (alterPitch){
            s.source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        } else {
            s.source.pitch = 1.0f;
        }

        s.source.Play();

        return s;
    }

    public Sound PlayPlayerSound (Sound s)
	{
        if (s == null) return null;
        s.source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        s.source.Play();

        return s;
	}

    // Purpose: Begin playing a sound group with given name
    // Will play all the sounds in a group in a row
    // If there is a sound in the group set to loop, it will begin to loop and will NOT play the following sounds
    // Set noLoop to true if you want the group to play through without any looping
    public ObjectSoundGroup PlayGroup(string name, bool noLoop = false)
    {
        // Debug.Log("Playing SoundGroup: " + name);

        // find the SoundGroup with the given name
        ObjectSoundGroup sg = FindSoundGroup(name);

        double startDelay = 0.5;

        // return null if SoundGroup was not found
        if (sg == null) return null;

        // check there is no sound already playing
        if (GetCurrentlyPlayingSound(sg) != null)
        {
            Debug.LogWarning("A sound in this group is already playing");
            return sg;
        }

        // reset clips that loop
        foreach (Sound s in sg.sounds) s.source.loop = noLoop ? false : sg.loopingClip == s.name;

        List<double> delayList = GetDelays(sg);
        double totalDelay = 0;

        double startTime = startDelay + AudioSettings.dspTime;

        foreach (Sound s in sg.sounds)
        {
            // if (s.index == 0) s.source.Play();
            // else s.source.PlayScheduled(AudioSettings.dspTime + delayList[s.index]);
            s.source.PlayScheduled(startTime + totalDelay);
            totalDelay += s.length;
            if (s.source.loop) break;
        }

        return sg;
    }

    // Purpose: If there is a looping track playing, turn off looping
    // and continue playing the rest of the sounds in the group
    // if there are any
    // Set continuePlaying to false if you don't want the clips after the looping clip to play
    public void EndLoop(string name, bool continuePlaying = true)
    {
        ObjectSoundGroup sg = FindSoundGroup(name);

        if (sg == null)
        {
            Debug.Log("AudioManager: EndLoop: Group not found: " + name);
            return;
        }

        // turn off looping in this group
        foreach (Sound s in sg.sounds) s.source.loop = false;

        Sound currentlyPlaying = GetCurrentlyPlayingSound(sg);
        List<double> delayList = GetDelaysFromPlaying(sg);

        foreach (double d in delayList)
        {
            Debug.Log("delayList: " + d);
        }

        for (int i = currentlyPlaying.index + 1; i < sg.sounds.Length; i++)
        {
            sg.sounds[i].source.PlayScheduled(AudioSettings.dspTime + delayList[i]);
        }
    }

    // Purpose: Play a random sound in a sound group
    public Sound PlayRandomSoundInGroup(string name, bool alterPitch = false)
    {
        return PlayRandomSoundInGroup(FindSoundGroup(name), alterPitch);
    }

    public Sound PlayRandomSoundInGroup(ObjectSoundGroup sg, bool alterPitch = false)
    {
        if (sg == null || sg.sounds.Length == 0) return null;

        Sound randomSound = sg.sounds[UnityEngine.Random.Range(0, sg.sounds.Length)];

        Play(randomSound, alterPitch);

        return randomSound;
    }
    public Sound PlayRandomPlayerSoundInGroup(string name)
    {
        return PlayRandomPlayerSoundInGroup(FindSoundGroup(name));
    }

    public Sound PlayRandomPlayerSoundInGroup(ObjectSoundGroup sg)
	{
        if (sg == null || sg.sounds.Length == 0) return null;

        Sound randomSound = sg.sounds[UnityEngine.Random.Range(0, sg.sounds.Length)];

        PlayPlayerSound(randomSound);

        return randomSound;
	}

    public Sound PlaySoundInGroup(string sgName, string sName)
    {
        ObjectSoundGroup sg = FindSoundGroup(sgName);
        return PlaySoundInGroup(sg, sName);
    }

    public Sound PlaySoundInGroup(ObjectSoundGroup sg, string sName)
    {
        Sound s = FindSoundInGroup(sg, sName);
        if (s == null) return null;
        Play(s);
        return s;
    }

    public void StopAll()
    {
        foreach (ObjectSoundGroup sg in soundGroups)
        {
            foreach (Sound s in sg.sounds)
            {
                s.source.Stop();
            }
        }
    }
}
