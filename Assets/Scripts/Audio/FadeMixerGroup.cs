using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

// FadeMixerGroup class described in https://gamedevbeginner.com/ultimate-guide-to-playscheduled-in-unity/#how_to_schedule
// Purpose: Smoothly fade the volume of different audio groups to a specific volume in a given time
// TODO: Calling a second fade before the first one is finished leads to strange activity
public class FadeMixerGroup
{
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
