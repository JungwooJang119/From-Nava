using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Simple Audio Manager, a sweet relief for sound lovers <3

public class AudioControl : Singleton<AudioControl> {

	private float musicVolume = 0.5f;
	private float sfxVolume = 1.0f;
	// Adjust the spacial distribution of sound in-game;
	private float minSFXDistance = 10;
	private float maxSFXDistance = 20;

	// Employs Travis and Chase's method;                                    
	void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

	// Declaring Audio Sources for music and sfx;
	[SerializeField] private AudioSource _musicSource;
	// Declaring arrays for audio files;
	// The files must be assigned in the inspector;
	[SerializeField] private Sound[] musicSounds, sfxSounds;

	// Method to play MUSIC TRACKS. Call syntax: AudioControl.Instance.PlayMusic(name);
	// Takes a string 'name', which corresponds to the name of a track in musicSounds[],
	// and a boolean 'shouldLoop' to define whether the track shoudl loop;
	// Plays the track requested if found. This source supports a single music track at a time;
	public AudioSource PlayMusic(string name, bool shouldLoop = true) {
		if (shouldLoop) {
			_musicSource.loop = true;
		} else {
			_musicSource.loop = false;
		}
		_musicSource.volume = musicVolume;
		Sound sn = Array.Find(musicSounds, item => item.name == name);

		if (sn != null) {
			_musicSource.clip = sn.clip;
			_musicSource.Play();
			return _musicSource;
		}
		else {
			Debug.LogWarning("Clip string is wrong");
			return null;
		}
	}

	// Method to play SOUND EFFECTS. Call syntax: AudioControl.Instance.PlaySFX(name, sender);
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[],
	// and a 'sender' GameObject reference (the object to which the SFX object will be childed;
	// May also output a reference to the given audio source if the parameter is provided;
	// Plays the sound requested if found. May be used several times to call multiple sounds;
	// This version returns the length of the sound played;
	public float PlaySFX(string name, GameObject sender, out AudioSource optionalSourceRef, float pitchRChange = 0) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			optionalSourceRef = SetUpSFX(sn.clip, sender, pitchRChange);
			return sn.clip.length;
		} else {
			optionalSourceRef = null;
			Debug.LogWarning("Clip string is wrong");
			return -1;
		}
	}

	// Overloaded version of the method above that does not require an out parameter;
	public float PlaySFX(string name, GameObject sender, float pitchRChange = 0) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			SetUpSFX(sn.clip, sender, pitchRChange);
			return sn.clip.length;
		} else {
			Debug.LogWarning("Clip string is wrong");
			return -1;
		}
	}

	// Method to play SOUND EFFECTS. Call syntax: AudioControl.Instance.PlaySFX(name);
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[];
	// This version returns null and it's global, for use in UI elements or global SFXs;
	public void PlayVoidSFX(string name, float pitchRChange = 0) {
		PlaySFX(name, GameObject.Find("Main Camera"), pitchRChange);
	}

	// Method to adjust the music volume. Called from VolumeSliders.cs;
	public void SetMusicVolume(float value) {
		musicVolume = value;
		_musicSource.volume = value;
	}

	// Method to adjust the SFX volume. Called from VolumeSliders.cs;
	public void SetSFXVolume(float value) {
		sfxVolume = value;
	}

	// Method to fade away the music.
	public void FadeMusic(bool stopsMusic, bool stopsAbruptly = false) {
		if (!stopsAbruptly) {
			StartCoroutine(_FadeMusic(stopsMusic));
		} else {
			_musicSource.Stop();
		}
	}

	// Coroutine to fade away the music. Hopefully more efficient than running a bool in Update;
	IEnumerator _FadeMusic(bool stopsMusic) {
		while (_musicSource.volume > 0) {
			_musicSource.volume = Mathf.Max(0, _musicSource.volume - musicVolume / (0.5f / Time.deltaTime));
			yield return null;
		}
		if (stopsMusic) {
			_musicSource.Stop();
		}
	}

	// Method to spawn an AudioSource in game space to play the sfx;
	private AudioSource SetUpSFX(AudioClip sn, GameObject sender, float pitchRChange) {
		GameObject tempObject = new GameObject("TempSFX");
		tempObject.transform.SetParent(sender.transform);
		tempObject.transform.position = sender.transform.position;
		AudioSource tempAudioSource = tempObject.AddComponent<AudioSource>();
		tempAudioSource.volume = sfxVolume;
		tempAudioSource.pitch += UnityEngine.Random.Range(-pitchRChange, pitchRChange);
		tempAudioSource.rolloffMode = AudioRolloffMode.Custom;
		tempAudioSource.clip = sn;
		tempAudioSource.dopplerLevel = 0;
		tempAudioSource.spatialBlend = 1;
		tempAudioSource.spread = 180;
		tempAudioSource.minDistance = minSFXDistance;
		tempAudioSource.maxDistance = maxSFXDistance;
		tempAudioSource.Play();
		Destroy(tempObject, sn.length);
		return tempAudioSource;
	}
}