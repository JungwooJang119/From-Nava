using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Simple Audio Manager inspired by Rehope Games' AudioManager and Brackey's AudioManager;

public class AudioControl : Singleton<AudioControl> {

	private float volume = 1.0f;

	// Employs Travis and Chase's method;                                    
	void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

	// Declaring Audio Sources for music and sfx;
	[SerializeField] private AudioSource _musicSource, _sfxSource;
	// Declaring arrays for audio files;
	// The files must be assigned in the inspector;
	[SerializeField] private Sound[] musicSounds, sfxSounds;

	// Method to play MUSIC TRACKS. Call syntax: AudioControl.Instance.PlayMusic(name);
	// Takes a string 'name', which corresponds to the name of a track in musicSounds[];
	// Plays the track requested if found. This source supports a single music track at a time;
	public void PlayMusic(string name) {
		_musicSource.volume = volume;
		Sound sn = Array.Find(musicSounds, item => item.name == name);

		if (sn != null) {
			_musicSource.clip = sn.clip;
			_musicSource.Play();
		}
		else {
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
		volume = value;
		_musicSource.volume = value;
	}

	// Method to adjust the SFX volume. Called from VolumeSliders.cs;
	public void SetSFXVolume(float value) {
		volume = value;
		_sfxSource.volume = value;
	}

	// Method to fade away the music.
	public void FadeMusic(bool stopsMusic) {
		StartCoroutine(_FadeMusic(stopsMusic));
	}

	// Coroutine to fade away the music. Hopefully more efficient than running a bool in Update;
	IEnumerator _FadeMusic(bool stopsMusic) {
		while (_musicSource.volume > 0) {
			_musicSource.volume = Mathf.Max(0, _musicSource.volume - volume / (0.5f / Time.deltaTime));
			yield return null;
		}
		if (stopsMusic) {
			_musicSource.Stop();
		}
	}
}