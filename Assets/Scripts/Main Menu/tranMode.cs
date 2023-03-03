using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Basic transition manager. For use in Main Menu buttons and scene transitions later on.
// Started from a Brackey's tutorial, but has been heavily modified since;

public class TranMode : MonoBehaviour {
	// Declaring animator object;
	public Animator transition;

	// Adjust to length of transition;
	public float transitionTime = 1f;

	// Method that advances to the next level. Called on Start button. Needs tweaking!
	public void LoadNext() {
		AudioControl.Instance._musicSource.Stop(); // Stops music from playing;
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
		// Retrieves index of next scene and loads it using LoadLevel() [see below];
	}

	// Method to fade the screen back in;
	public float FadeIn() {
		transition.SetTrigger("FadeIn");
		return transitionTime;
	}

	// Method to fade the screen to black;
	public float FadeOut() {
		transition.SetTrigger("FadeOut");
		return transitionTime;
	}

	// Coroutine for scene loading;
	IEnumerator LoadLevel(int levelIndex) {
		//Play animation;
		transition.SetTrigger("FadeOut");

		//Wait;
		yield return new WaitForSeconds(transitionTime);

		//Load Scene;
		SceneManager.LoadScene(levelIndex);
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