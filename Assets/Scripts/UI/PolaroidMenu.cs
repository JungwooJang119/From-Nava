using System.Collections;
using System.Collections.Generic;
using static CollectibleController;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PolaroidMenu : MonoBehaviour
{
	private CollectibleController controller;

	[SerializeField] private Sprite unclaimedPolaroid;
	private Dictionary<string, Sprite> polaroidSprites;

	void Awake() {
		controller = ReferenceSingleton.Instance.collectibleController;
		polaroidSprites = new Dictionary<string, Sprite>();
		foreach (Transform t in transform) {
			if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
				polaroidSprites[t.gameObject.name] = t.GetComponent<Image>().sprite;
			}
		}
	}

	void OnEnable() {
		foreach (Transform t in transform) {
			if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
				if (!controller.CheckClaimedStatus(CollectibleType.Polaroid, t.gameObject.name)) {
					var image = t.GetComponent<Image>();
					image.sprite = unclaimedPolaroid;
					t.GetComponent<Button>().interactable = false;
				} else {
					var image = t.GetComponent<Image>();
					image.sprite = polaroidSprites[t.gameObject.name];
					t.GetComponent<Button>().interactable = true;
				}
			}
		}
	}
}
