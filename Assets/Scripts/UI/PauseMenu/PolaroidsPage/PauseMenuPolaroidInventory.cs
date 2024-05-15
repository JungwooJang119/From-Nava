using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollectibleController;

public class PauseMenuPolaroidInventory : MonoBehaviour {

	private CollectibleController controller;
	private PauseMenuPolaroidButton[] polaroidButtons;

	void Awake() {
		controller = ReferenceSingleton.Instance.collectibleController;
		polaroidButtons = GetComponentsInChildren<PauseMenuPolaroidButton>(true);
	}

	void OnEnable() {
		HashSet<ItemData> polaroidInventory = controller.GetItems<PolaroidData>();
		foreach (PauseMenuPolaroidButton pb in polaroidButtons) pb.gameObject.SetActive(polaroidInventory.Contains(pb.PolaroidData));
	}
}
