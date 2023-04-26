using System.Collections;
using System.Collections.Generic;
using static CollectibleController;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DedicatedMenus : MonoBehaviour
{
	private CollectibleController controller;
	public enum MenuType {
		Polaroid,
		Tutorial,
		LabReport,
	} [SerializeField] MenuType menuType;

	[SerializeField] private Sprite unclaimedPolaroid;
	private Dictionary<string, Sprite> polaroidSprites;
	private Dictionary<string, string> textData;

	void Awake() {
		controller = ReferenceSingleton.Instance.collectibleController;
		if (menuType == MenuType.Polaroid) {
			polaroidSprites = new Dictionary<string, Sprite>();
			foreach (Transform t in transform) {
				if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
					polaroidSprites[t.gameObject.name] = t.GetComponent<Image>().sprite;
				}
			}
		} else {
			textData = new Dictionary<string, string>();
			foreach (Transform t in transform) {
				if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
					textData[t.gameObject.name] = t.GetComponentInChildren<TextMeshProUGUI>().text;
				}
			}
		}
	}

	void OnEnable() {
		switch(menuType) {
			case MenuType.Polaroid:
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
				} break;
			case MenuType.Tutorial:
				foreach (Transform t in transform) {
					if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
						if (!controller.CheckClaimedStatus(CollectibleType.Tutorial, t.gameObject.name)) {
							t.GetComponentInChildren<TextMeshProUGUI>().text = "...";
							t.GetComponent<Button>().interactable = false;
						} else {
							t.GetComponentInChildren<TextMeshProUGUI>().text = textData[t.gameObject.name];
							t.GetComponent<Button>().interactable = true;
						}
					}
				} break;
			case MenuType.LabReport:
				foreach (Transform t in transform) {
					if (t.gameObject.name != "Back Arrow" && t.gameObject.name != "Menu Title") {
						if (!controller.CheckClaimedStatus(CollectibleType.Report, t.gameObject.name)) {
							t.GetComponentInChildren<TextMeshProUGUI>().text = "...";
							t.GetComponent<Button>().interactable = false;
						} else {
							t.GetComponentInChildren<TextMeshProUGUI>().text = textData[t.gameObject.name];
							t.GetComponent<Button>().interactable = true;
						}
					}
				} break;
		}
	}
}
