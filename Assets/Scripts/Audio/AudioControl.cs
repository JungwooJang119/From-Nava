using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Simple Audio Manager, a sweet relief for sound lovers <3
/// </summary>

public class AudioControl : Singleton<AudioControl> {

	private float musicVolume = 0.5f;
	private float sfxVolume = 1.0f;
	// Adjust the spacial distribution of sound in-game;
	[SerializeField] private float minSFXDistance = 10;
	[SerializeField] private float maxSFXDistance = 20;

	private const string NULL_CLIP_TEXT = "Invalid clip string passed";
	private readonly float volumeChangeRate = 0.01f;

	// Employs Travis and Chase's method;                                    
	void Awake() {
		secondaryMusicSource = Instantiate(mainMusicSource.gameObject, transform).GetComponent<AudioSource>();
		secondaryMusicSource.gameObject.name = "SecondaryMusicSource";
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

	//Sets the MainMusicSource time to 0. For some reason this fixes the Audio issue at the ending.
	//The issue was with Swap, where time was set to the time of the other track. For some reason this breaks things. 
	public void ResetTime() {
		mainMusicSource.time = 0;
	}

    public void CheckMusic() {
		int buildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		StopAllCoroutines();

		switch (buildIndex) {
			case 0:
				PlayMusic("Main");
				break;
			case 1:
				StartCoroutine(LoadMusic());
				break;
        }
	}

    // Declaring Audio Sources for music and sfx;
    [SerializeField] private AudioSource mainMusicSource, secondaryMusicSource, sfxSource;
	// Declaring arrays for audio files;
	// The files must be assigned in the inspector;
	[SerializeField] private Sound[] musicSounds, sfxSounds;

	private Coroutine activeInterpolation;

	private AudioClip FetchClip(string name, Sound[] sounds) {
		Sound sn = Array.Find(sounds, item => item.name == name);
		if (sn == null) throw new Exception(NULL_CLIP_TEXT);
		return sn.clip;
	}

	/// <summary>
	/// Method to play music tracks;
	/// <br></br> The music source supports a single music track at a time;
	/// </summary>
	/// <param name="name"> Name of the clip to play; </param>
	/// <param name="shouldLoop"> Whether the music should loop; </param>
	/// <returns> Returns the request track if found track requested if found; </returns>
	public AudioSource PlayMusic(string name, bool shouldLoop = true) {
		mainMusicSource.loop = shouldLoop;
		mainMusicSource.volume = musicVolume;
		AudioClip clip = FetchClip(name, musicSounds);
		mainMusicSource.clip = clip;
		mainMusicSource.Play();
		return mainMusicSource;
	}

	/// <summary>
	/// Method to play local sound effects;
	/// <br></br> Drops an object containing the audio source in worldspace;
	/// </summary>
	/// <param name="name"> Name of the clip to play; </param>
	/// <param name="sender"> GameObject to which the SFX will be childed; </param>
	/// <param name="optionalSourceRef"> A reference to the Audio Source used, if requested; </param>
	/// <param name="pitchRChange"> Pitch variance of the clip (additive); </param>
	/// <param name="volumeMultiplier"> Volume of the clip (between 0 and 1); </param>
	/// <returns> Returns the length of the clip played if one is found; </returns>
	public float PlaySFX(string name, GameObject sender, out AudioSource optionalSourceRef,
						 float pitchRChange = 0, float volumeMultiplier = 1) {

		AudioClip clip = FetchClip(name, sfxSounds);
		optionalSourceRef = SetUpSFX(clip, sender, pitchRChange, volumeMultiplier);
		return clip.length;
	}

	/// <summary>
	/// Method to play local sound effects;
	/// <br></br> Drops an object containing the audio source in worldspace;
	/// </summary>
	/// <param name="name"> Name of the clip to play; </param>
	/// <param name="sender"> GameObject to which the SFX will be childed; </param>
	/// <param name="pitchRChange"> Pitch variance of the clip (additive); </param>
	/// <param name="volumeMultiplier"> Volume of the clip (between 0 and 1); </param>
	/// <returns> Returns the length of the clip played if one is found; </returns>
	public float PlaySFX(string name, GameObject sender, float pitchRChange = 0, float volumeMultiplier = 1) {
		Sound sn = Array.Find(sfxSounds, item => item.name == name);

		AudioClip clip = FetchClip(name, sfxSounds);
		SetUpSFX(clip, sender, pitchRChange, volumeMultiplier);
		return clip.length;
	}

	/// <summary>
	/// Method to play global sound effects;
	/// <br></br> The sounds are played on the manager's audio source;
	/// </summary>
	/// <param name="name"> Name of the clip to play; </param>
	/// <param name="pitchRChange"> Pitch variance of the clip (additive); </param>
	/// <param name="volumeMultiplier"> Volume of the clip (between 0 and 1); </param>
	/// <returns> Returns the length of the clip played if one is found; </returns>
	public void PlayVoidSFX(string name, float pitchRChange = 0, float volumeMultiplier = 1) {

		AudioClip clip = FetchClip(name, sfxSounds);
		sfxSource.pitch = 1f + UnityEngine.Random.Range(-pitchRChange, pitchRChange);
		sfxSource.volume = sfxVolume * volumeMultiplier;
		sfxSource.PlayOneShot(clip);
	}

	/// <summary>
	/// Method to play global sound effects;
	/// <br></br> The sounds are played on the manager's audio source;
	/// </summary>
	/// <param name="name"> Name of the clip to play; </param>
	/// <param name="optionalLengthReturn"> Optional clip length output; </param>
	/// <param name="pitchRChange"> Pitch variance of the clip (additive); </param>
	/// <param name="volumeMultiplier"> Volume of the clip (between 0 and 1); </param>
	public void PlayVoidSFX(string name, out float optionalLengthReturn,
							float pitchRChange = 0, float volumeMultiplier = 1) {

		AudioClip clip = FetchClip(name, sfxSounds);
		sfxSource.pitch = 1f + UnityEngine.Random.Range(-pitchRChange, pitchRChange);
		sfxSource.volume = sfxVolume * volumeMultiplier;
		sfxSource.PlayOneShot(clip);
		optionalLengthReturn = clip.length;
	}

	/// <summary>
	/// Method to fade away the music.
	/// </summary>
	/// <param name="stopsMusic"> Whether the music should be stopped (true), or paused (false); </param>
	/// <param name="stopsAbruptly"> Whether the fadeout happens immediately (true), or with linear interpolation (false); </param>
	public void FadeMusic(bool stopsMusic, bool stopsAbruptly = false) {
		StopAllCoroutines();
		if (stopsAbruptly) mainMusicSource.Stop();
		else StartCoroutine(_FadeMusic(stopsMusic));
	}

	public void ResumeMusic() => StartCoroutine(_ResumeMusic());

	public void InterpolateMusicTracks(string name, bool shouldLoop = true) {
		StopAllCoroutines();
		AudioClip clip = FetchClip(name, musicSounds);
		//if (activeInterpolation != null) StopCoroutine(activeInterpolation);
		activeInterpolation = StartCoroutine(_InterpolateMusicTracks(clip, shouldLoop));
	}

	IEnumerator _InterpolateMusicTracks(AudioClip newTrack, bool nextLoop) {
		float lerp = 1;
		SetUpSecondaryMusicClip(newTrack);
		while (lerp > 0) {
			mainMusicSource.volume = musicVolume * lerp;
			secondaryMusicSource.volume = musicVolume * (1 - lerp);
			lerp = Mathf.MoveTowards(lerp, 0, volumeChangeRate);
			yield return null;
		} SwapPrimaryMusicSource(nextLoop);
    }

	private void SetUpSecondaryMusicClip(AudioClip newTrack) {
		secondaryMusicSource.clip = newTrack;
		secondaryMusicSource.time = mainMusicSource.time;
		secondaryMusicSource.Play();
	}

	private void SwapPrimaryMusicSource(bool shouldLoop) {
		AudioSource oldSource = mainMusicSource;
		mainMusicSource = secondaryMusicSource;
		mainMusicSource.loop = shouldLoop;
		secondaryMusicSource = oldSource;
		secondaryMusicSource.Stop();
    }

	// Coroutine to fade away the music. Hopefully more efficient than running a bool in Update;
	IEnumerator _FadeMusic(bool stopsMusic) {
		while (mainMusicSource.volume > 0) {
			mainMusicSource.volume = Mathf.MoveTowards(mainMusicSource.volume, 0, volumeChangeRate);
			yield return null;
		}
		if (stopsMusic) {
			mainMusicSource.Stop();
			mainMusicSource.volume = musicVolume;
		}
	}

	IEnumerator _ResumeMusic() {
		if (mainMusicSource) mainMusicSource.UnPause();
		while (mainMusicSource.volume < musicVolume) {
			mainMusicSource.volume = Mathf.MoveTowards(mainMusicSource.volume, musicVolume, volumeChangeRate);
			yield return null;
		}
	}

	IEnumerator LoadMusic() {
		var musicSource = PlayMusic("Exploration Opening", false);
		while (musicSource.isPlaying) {
			// Debug.LogWarning("Loop running");
			yield return null;
		}
		PlayMusic("Exploration");
	}

	/// <summary>
	/// Method to spawn an AudioSource in game space to play a SFX;
	/// </summary>
	/// <param name="sn"> AudioClip to play; </param>
	/// <param name="sender"> Object to which the spawned object will be childed; </param>
	/// <param name="pitchRChange"> Additive pitch variation for the SFX; </param>
	/// <param name="volumeMultiplier"> Multiplier for the SFX's volume (between 0 and 1); </param>
	/// <returns> The audio source attached to the GameObject; </returns>
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

	/// <summary>
	/// Update the music volume utilized by the manager;
	/// </summary>
	/// <param name="value"> New music volume (between 0 and 1); </param>
	public void SetMusicVolume(float value) {
		musicVolume = value;
		mainMusicSource.volume = value;
	}

	public float GetMusicVolume() {
		return musicVolume;
	}

	/// <summary>
	/// Update the sfx volume utilized by the manager;
	/// </summary>
	/// <param name="value"> New music volume (between 0 and 1); </param>
	public void SetSFXVolume(float value) {
		sfxVolume = value;
		sfxSource.volume = value;
	}

	public float GetSFXVolume() {
		return sfxVolume;
	}
}

[System.Serializable]
public class Sound {
	public string name;
	public AudioClip clip;
}