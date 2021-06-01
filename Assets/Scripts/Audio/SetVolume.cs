using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
	public string channelName;

	public Slider slider;

	private void Start() {
		float originalVolume;
		mixer.GetFloat(channelName, out originalVolume);
		slider.value = Mathf.Pow(10, originalVolume / 20);
	}

    public void SetLevel(float sliderValue)
	{
		mixer.SetFloat(channelName, Mathf.Log10(sliderValue) * 20);
	}
}
