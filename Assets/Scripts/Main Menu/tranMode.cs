using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Basic transition manager. For use in Main Menu buttons and scene transitions later on.
// Started from a Brackey's tutorial, but has been heavily modified since;

public class tranMode : MonoBehaviour {
	// Declaring animator object;
	public Animator transition;

	// Adjust to length of transition;
	public float transitionTime = 1f;

	// Method that advances to the requested level.
	// Takes in the index of the level one may want to call.
	// I reccommend setting up a String reference somewhere once more levels are added.
	public void StartGame() {
		StartCoroutine(LoadLevel());
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

	public float DarkenIn() {
		transition.SetTrigger("DarkenIn");
		return transitionTime;
	}
	public float DarkenOut() {
		transition.SetTrigger("DarkenOut");
		return transitionTime;
	}

	// Coroutine for scene loading;
	IEnumerator LoadLevel() {
		//Play animation;
		transition.SetTrigger("FadeOut");

		//Wait;
		yield return new WaitForSeconds(transitionTime);

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