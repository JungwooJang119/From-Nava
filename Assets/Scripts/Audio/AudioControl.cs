using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Simple Audio Manager, a sweet relief for sound lovers <3

public class AudioControl : Singleton<AudioControl> {

	// Scripts containing calls to Audio Manager:
	// tranMode.cs (music); SpellCastManager.cs (spell cast); all [Spell]Behavior scripts (spell collision);
	// LaserTerminal.cs, LaserCaster.cs, Receiver.cs, LabReport.cs, SpellMachine.cs;

	private float musicVolume = 0.5f;
	private float sfxVolume = 1.0f;
	// Adjust the spacial distribution of sound in-game;
	[SerializeField] private float minSFXDistance = 10;
	[SerializeField] private float maxSFXDistance = 20;

	// Employs Travis and Chase's method;                                    
	void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

	// Declaring Audio Sources for music and sfx;
	[SerializeField] private AudioSource musicSource, sfxSource;
	// Declaring arrays for audio files;
	// The files must be assigned in the inspector;
	[SerializeField] private Sound[] musicSounds, sfxSounds;

	// Method to play music tracks;
	// Takes a string 'name', which corresponds to the name of a track in musicSounds[],
	// and a boolean 'shouldLoop' to define whether the track shoudl loop;
	// Plays the track requested if found. This source supports a single music track at a time;
	public AudioSource PlayMusic(string name, bool shouldLoop = true) {
		if (shouldLoop) {
			musicSource.loop = true;
		} else {
			musicSource.loop = false;
		}
		musicSource.volume = musicVolume;
		Sound sn = Array.Find(musicSounds, item => item.name == name);

		if (sn != null) {
			musicSource.clip = sn.clip;
			musicSource.Play();
			return musicSource;
		}
		else {
			Debug.LogWarning("Clip string is wrong");
			return null;
		}
	}

	// Method to play local sound effects. Drops an object containing the audio source in worldspace; 
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[],
	// and a 'sender' GameObject reference (the object to which the SFX object will be childed;
	// May also output a reference to the given audio source if the parameter is provided;
	// Plays the sound requested if found. May be used several times to call multiple sounds;
	// This version returns the length of the sound played;
	public float PlaySFX(string name, GameObject sender, out AudioSource optionalSourceRef, 
						 float pitchRChange = 0, float volumeMultiplier = 1) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			optionalSourceRef = SetUpSFX(sn.clip, sender, pitchRChange, volumeMultiplier);
			return sn.clip.length;
		} else {
			optionalSourceRef = null;
			Debug.LogWarning("Clip string is wrong");
			return -1;
		}
	}

	// Overloaded version of the method above that does not require an out parameter;
	public float PlaySFX(string name, GameObject sender, float pitchRChange = 0, float volumeMultiplier = 1) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			SetUpSFX(sn.clip, sender, pitchRChange, volumeMultiplier);
			return sn.clip.length;
		} else {
			Debug.LogWarning("Clip string is wrong");
			return -1;
		}
	}

	// Method to play global sound effects. The sounds are played on the manager's audio source;
	// Takes a string 'name', which corresponds to the name of a sound in sfxSounds[];
	// This version returns null and produces a GLOBAL sound;
	public void PlayVoidSFX(string name, float pitchRChange = 0, float volumeMultiplier = 1) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			sfxSource.pitch = 1f + UnityEngine.Random.Range(-pitchRChange, pitchRChange);
			sfxSource.volume = sfxVolume * volumeMultiplier;
			sfxSource.PlayOneShot(sn.clip);
		} else {
			Debug.LogWarning("Clip string is wrong");
		}
	}

	// Overloaded version of the method above that requires an out parameter (with clip length info);
	public void PlayVoidSFX(string name, out float optionalLengthReturn,
							float pitchRChange = 0, float volumeMultiplier = 1) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		if (sn != null) {
			sfxSource.pitch = 1f + UnityEngine.Random.Range(-pitchRChange, pitchRChange);
			sfxSource.volume = sfxVolume * volumeMultiplier;
			sfxSource.PlayOneShot(sn.clip);
			optionalLengthReturn = sn.clip.length;
		}
		else {
			optionalLengthReturn = 0;
			Debug.LogWarning("Clip string is wrong");
		}
	}

	// Method to adjust the music volume. Called from VolumeSliders.cs;
	public void SetMusicVolume(float value) {
		musicVolume = value;
		musicSource.volume = value;
	}

	// Method to adjust the SFX volume. Called from VolumeSliders.cs;
	public void SetSFXVolume(float value) {
		sfxVolume = value;
		sfxSource.volume = value;
	}

	// Method to fade away the music.
	public void FadeMusic(bool stopsMusic, bool stopsAbruptly = false) {
		if (!stopsAbruptly) {
			StartCoroutine(_FadeMusic(stopsMusic));
		} else {
			musicSource.Stop();
		}
	}

	public void ResumeMusic() {
		StartCoroutine(_ResumeMusic());
	}

	// Coroutine to fade away the music. Hopefully more efficient than running a bool in Update;
	IEnumerator _FadeMusic(bool stopsMusic) {
		while (musicSource.volume > 0) {
			musicSource.volume = Mathf.Max(0, musicSource.volume - musicVolume / (0.5f / Time.deltaTime));
			yield return null;
		}
		if (stopsMusic) {
			musicSource.Stop();
		}
	}

	IEnumerator _ResumeMusic() {
		if (musicSource) musicSource.UnPause();
		while (musicSource.volume < musicVolume) {
			musicSource.volume = Mathf.Min(musicVolume, musicSource.volume + musicVolume / (0.5f / Time.deltaTime));
			yield return null;
		} 
	}

	// Method to spawn an AudioSource in game space to play the sfx;
	private AudioSource SetUpSFX(AudioClip sn, GameObject sender, float pitchRChange, float volumeMultiplier) {
		GameObject tempObject = new GameObject("TempSFX");
		tempObject.transform.SetParent(sender.transform);
		tempObject.transform.position = sender.transform.position;
		AudioSource tempAudioSource = tempObject.AddComponent<AudioSource>();
		tempAudioSource.volume = sfxVolume * volumeMultiplier;
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