using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSkipLogic : MonoBehaviour
{

    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private GameObject skipText;
    bool AtConfirmationToSkip = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 3) {
            SkipIntro();
		} else if (AtConfirmationToSkip && Input.anyKeyDown) {
            skipText.SetActive(false);
			AtConfirmationToSkip = false;
		}
    }

    private void SkipIntro() {
        if (!AtConfirmationToSkip) {
            AtConfirmationToSkip = true;
            skipText.SetActive(true);
        } else if (AtConfirmationToSkip) {
            skipText.SetActive(false);
            transitionManager.SetTransitionColor(Color.black);
			transitionManager.FadeOut();
			transitionManager.LoadLevel();
        }
    }


}
