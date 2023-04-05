using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextRoom : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    private tranMode tm;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<tranMode>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
			StartCoroutine(SimulateLoad(other.gameObject));
        }
    }
    IEnumerator SimulateLoad(GameObject player) {
        var wait = tm.FadeOut();
        yield return new WaitForSeconds(wait);
		player.transform.position = spawn.transform.position;
        tm.FadeIn();
	}
    
}
