using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
	public string channelName;

    public void SetLevel(float sliderValue)
	{
		mixer.SetFloat(channelName, Mathf.Log10(sliderValue) * 20);
	}
}
