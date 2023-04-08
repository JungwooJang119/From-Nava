using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Basic transition manager. For use in Main Menu buttons and transitions.

public class tranMode : MonoBehaviour {

	// Adjust to length of transition;
	[SerializeField] private float shortTransitionTime = 0.5f;
	[SerializeField] private float longTransitionTime = 1f;
	private float currentTransitionTime;
	private float alpha;
	private float target;
	private CanvasGroup fadeScreen;
	private float timecheck;

	void Start() {
		fadeScreen = GetComponentInChildren<CanvasGroup>();
		alpha = 1f;
		target = 0;
		currentTransitionTime = longTransitionTime;
	}

	void Update() {
		if (alpha != target) {
			timecheck += Time.deltaTime;
			if (alpha < target) {
				alpha = Mathf.Min(target, alpha + Time.deltaTime * 1f/currentTransitionTime);
			}
			else if (alpha > target) {
				alpha = Mathf.Max(target, alpha - Time.deltaTime * 1f/currentTransitionTime);
			}
			fadeScreen.alpha = alpha;
		} else if (target == 0) {
			fadeScreen.blocksRaycasts = false;
			fadeScreen.interactable = false;
		}
	}

	// Method that advances to the requested level.
	// Takes in the index of the level one may want to call.
	// I reccommend setting up a String reference somewhere once more levels are added.
	public void StartGame() {
		StartCoroutine(LoadLevel());
	}

	// Method to fade the screen back in;
	public float FadeIn() {
		currentTransitionTime = shortTransitionTime;
		target = 0;
		return currentTransitionTime;
	}

	// Method to fade the screen to black;
	public float FadeOut() {
		currentTransitionTime = shortTransitionTime;
		target = 1f;
		fadeScreen.blocksRaycasts = true;
		fadeScreen.interactable = true;
		return currentTransitionTime;
	}

	public float DarkenIn() {
		currentTransitionTime = longTransitionTime;
		target = 0;
		return currentTransitionTime;
	}

	public float DarkenOut() {
		currentTransitionTime = longTransitionTime;
		target = 0.5f;
		return currentTransitionTime;
	}

	// Coroutine for scene loading;
	IEnumerator LoadLevel() {
		//Play animation;
		target = 1;

		//Wait;
		yield return new WaitForSeconds(currentTransitionTime);

		//Load Scene;
		SceneManager.LoadScene(1);
	}

	// Method to quit the game. Called on Quit button. Exits play mode if testing;
	public void Quit() {
		#if UNITY_STANDALONE
				Application.Quit();
		#endif
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}