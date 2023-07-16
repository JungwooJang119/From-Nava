using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainExit : MonoBehaviour
{
    private tranMode tm;
    [SerializeField] private Door door;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<tranMode>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
			if (door.isOpen) {
                StartCoroutine(SimulateLoad(other.gameObject));
            }
        }
    }
    IEnumerator SimulateLoad(GameObject player) {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.DeactivateMovement();
        var wait = tm.FadeOut();
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(3);
	}
}
