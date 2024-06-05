using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSkipLogic : MonoBehaviour
{

    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private GameObject skipText;
    private TextMeshProUGUI text;
    private bool AtConfirmationToSkip = false;

    private void Start()
    {
        text = skipText.GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update() {
        if (!AtConfirmationToSkip && Input.anyKeyDown) {
            StartCoroutine(ToggleText(true));
        } else {
            if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 3) {
                SkipIntro();
            } else if (Input.anyKeyDown) {
                StartCoroutine(ToggleText(false));
            }
        }
    }

    private void SkipIntro() {
        skipText.SetActive(false);
        transitionManager.SetTransitionColor(Color.black);
		transitionManager.FadeOut();
		transitionManager.LoadLevel();
    }

    private IEnumerator ToggleText(bool status) {
        if (status) {
            skipText.SetActive(true);
            AtConfirmationToSkip = true;
            text.alpha = 0;
            while (text.alpha < 1) {
                text.alpha += .1f;
                yield return new WaitForSeconds(.01f);
            }
        } else {
            AtConfirmationToSkip = false;
            text.alpha = 1;
            while (text.alpha > 0) {
                text.alpha -= .1f;
                yield return new WaitForSeconds(.01f);
            }
            skipText.SetActive(false);
        }
    }


}
