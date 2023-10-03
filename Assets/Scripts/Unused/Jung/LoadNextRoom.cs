using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextRoom : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    private tranMode tm;

    [SerializeField] private GameObject[] _objects;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<tranMode>();
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
		player.transform.position = spawn.transform.position;
        controller.ChangeSpawn(spawn);
        foreach (GameObject objList in _objects) {
			objList.GetComponent<ControllerObject>().StartReset();
		}
        yield return new WaitForSeconds(wait);
        RoomDisablerControl.Instance.ChangeRoomsState(spawn);
        tm.FadeIn();
        controller.ActivateMovement();
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell") {
            return;
        }
	}
    
}
