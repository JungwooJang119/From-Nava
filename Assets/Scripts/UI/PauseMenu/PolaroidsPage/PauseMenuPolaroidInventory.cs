using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollectibleController;

public class PauseMenuPolaroidInventory : MonoBehaviour {

	private CollectibleController controller;
	private Dictionary<string, GameObject> polaroidDict;

	void Awake() {
		controller = ReferenceSingleton.Instance.collectibleController;
		polaroidDict = new Dictionary<string, GameObject>();
		foreach (Transform t in transform) {
			polaroidDict[t.gameObject.name] = t.gameObject;
		}
	}

	void OnEnable() {
		foreach (Transform t in transform) {
			t.gameObject.SetActive(controller.CheckClaimedStatus(CollectibleType.Polaroid, t.gameObject.name));
		}
	}
}
