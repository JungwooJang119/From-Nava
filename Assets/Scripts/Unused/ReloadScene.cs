using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private GameObject[] _firewoodControllers;
    [SerializeField] private GameObject[] _mirrorControllers;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell") {
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
	}
}
