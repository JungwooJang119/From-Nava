using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script for volume sliders. Changes volume of _musicSource and _sfxSource
// in AudioControl.cs, and the AudioListener's volume on Main Camera (master).

public class VolumeSliders : MonoBehaviour {
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;

	// Set the volume of the Music and SFX sources to the initial values of the sliders;
	void Start() {
		masterSlider.value = AudioListener.volume;
		musicSlider.value = AudioControl.Instance.GetMusicVolume();
		sfxSlider.value = AudioControl.Instance.GetSFXVolume();
		masterSlider.onValueChanged.AddListener(SetMasterVolume);
		musicSlider.onValueChanged.AddListener(SetMusicVolume);
		sfxSlider.onValueChanged.AddListener(SetSFXVolume);
	}

	// Changes volume of the AudioListener in the Main Camera [Master Volume];
	public void SetMasterVolume(float value) {
		AudioListener.volume = value;
	}

	// Changes volume of _musicSource [audio source] in AudioControl.cs;
	public void SetMusicVolume(float value) {
		AudioControl.Instance.SetMusicVolume(value);
    }

	// Changes volume of _sfxSource [audio source] in AudioControl.cs;
	public void SetSFXVolume(float value) {
		AudioControl.Instance.SetSFXVolume(value);
	}

	public Slider GetMasterSlider() {
		return masterSlider;
	}

	public Slider GetMusicSlider() {
		return musicSlider;
	}

	public Slider GetSFXSlider() {
		return sfxSlider;
    }
}
