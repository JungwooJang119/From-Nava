using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for volume sliders. Changes volume of _musicSource and _sfxSource
// in AudioControl.cs, and the AudioListener's volume on Main Camera (master).

public class VolumeSliders : MonoBehaviour
{
    public Slider _masterSlider, _musicSlider, _sfxSlider;

	// Set the volume of the Music and SFX sources to the initial values of the sliders;
	void Start() {
		AudioControl.Instance.SetMusicVolume(_musicSlider.value);
		AudioControl.Instance.SetSFXVolume(_sfxSlider.value);
	}

	// Changes volume of the AudioListener in the Main Camera [Master Volume];
	public void SetMasterVolume() {
		AudioListener.volume = _masterSlider.value;
	}

	// Changes volume of _musicSource [audio source] in AudioControl.cs;
	public void SetMusicVolume() {
		AudioControl.Instance.SetMusicVolume(_musicSlider.value);
    }

	// Changes volume of _sfxSource [audio source] in AudioControl.cs;
	public void SetSFXVolume() {
		AudioControl.Instance.SetSFXVolume(_sfxSlider.value);
	}
}
