using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private GameObject[] _firewoodControllers;
    [SerializeField] private GameObject[] _mirrorControllers;
    [SerializeField] private GameObject[] _objects;
    private TransitionManager transition;

    void Start() {
		transition = ReferenceSingleton.Instance.transition;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell" || other.tag == "WindBlast") {
            return;
        }
        foreach (GameObject firewoodPar in _firewoodControllers) {
            if (firewoodPar != null) {
                firewoodPar.GetComponent<FirewoodController>().PressPlate();
            }
		}
		foreach (GameObject mirrorPar in _mirrorControllers) {
			if (mirrorPar != null) {
				mirrorPar.GetComponent<MirrorController>().PressPlate();
			}
		}
        foreach (GameObject objList in _objects) {
			objList.GetComponent<ControllerObject>().StartReset();
		}

	}
}
