using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainExit : MonoBehaviour
{
    private TransitionManager tm;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<TransitionManager>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            StartCoroutine(SimulateLoad(other.gameObject));
        }
    }
    IEnumerator SimulateLoad(GameObject player) {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.DeactivateMovement();
        var wait = tm.FadeOut();
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(4);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(SimulateLoad(other.gameObject));
    }
}
