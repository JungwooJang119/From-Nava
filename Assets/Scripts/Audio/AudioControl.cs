using UnityEngine;
using System;

// Simple Audio Manager inspired by Rehope Games' AudioManager and Brackey's AudioManager;

public class AudioControl : Singleton<AudioControl> {

    // Employs Travis and Chase's method;                                    
    void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

	// Declaring Audio Sources for music and sfx;
	public AudioSource _musicSource, _sfxSource;
    // Declaring arrays for audio files;
    // The files must be assigned in the inspector;
    public Sound[] musicSounds, sfxSounds;

	void Start() {
        //PlayMusic("Theme Sample");
    }

	// Method to play MUSIC TRACKS. Call syntax: AudioControl.Instance.PlayMusic(name);
	// Takes a string 'name', which corresponds to the name of a track in musicSounds[];
	// Plays the track requested if found. This source supports a single music track at a time;
	public void PlayMusic(string name) {
		Sound sn = Array.Find(musicSounds, item => item.name == name);

		if (sn != null) {
			_musicSource.clip = sn.clip;
			_musicSource.Play();
		} else {
			Debug.Log("Clip string is wrong");
		}
	}

	// Method to play SOUND EFFECTS. Call syntax: AudioControl.Instance.PlaySFX(name);
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[];
	// Plays the sound requested if found. May be used several times to call multiple sounds;
	// This version returns the length of the sound played;
	public float PlaySFX(string name) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			_sfxSource.PlayOneShot(sn.clip);
			return sn.clip.length;
		}
		else {
			Debug.Log("Clip string is wrong");
			return -1;
		}
	}

	// Method to play SOUND EFFECTS. Call syntax: AudioControl.Instance.PlaySFX(name);
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[];
	// This version returns null, for use in UI elements that only support void methods;
	public void PlayVoidSFX(string name) {
		PlaySFX(name);
	}

	// Method to adjust the music volume. Called from VolumeSliders.cs;
	public void SetMusicVolume(float value) {
		_musicSource.volume = value;
	}

	// Method to adjust the SFX volume. Called from VolumeSliders.cs;
	public void SetSFXVolume(float value) {
		_sfxSource.volume = value;
	}
}