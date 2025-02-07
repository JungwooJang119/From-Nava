using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextRoom : MonoBehaviour
{
    [SerializeField] protected Transform spawn;
    protected TransitionManager tm;

    [SerializeField] protected GameObject[] _objects;
    private static GameObject auditor;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<TransitionManager>();
        if (auditor == null) {
            auditor = GameObject.Find("Auditor");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            if (string.Compare(spawn.name, "B1South") == 0 || string.Compare(spawn.name, "S5South") == 0) {
                auditor.GetComponent<Auditor>().updateEnterRoom(spawn.name);
            }
			StartCoroutine(SimulateLoad(other.gameObject));
        }
    }
    protected virtual IEnumerator SimulateLoad(GameObject player) {
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