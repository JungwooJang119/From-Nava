using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the access to the Lab Report and the events shown On-Screen.

public class LabReport : MonoBehaviour
{
	[SerializeField] private float range = 2;
	[SerializeField] static GameObject auditor;

	// Room Control;
	public event Action OnReportRead;
	private ClaimCollectible collectible;
	private bool awaitingCollectible;

	private string intKey = "space";				// Key used to trigger interactions;
	private Transform playerTransform;

	// Button Tutorial;
	[SerializeField] private GameObject buttonTutorial; // Reference to button tutorial pop-up;
	private GameObject tutInstance;		// Reference to instantiate text pop-up;
	private ButtonTutorial tutScript;   // Reference to instantiated text script;

	// Initialize variable references;
	void Start() {
		if (auditor == null) {
			auditor = GameObject.Find("Auditor");
		}
		collectible = GetComponent<ClaimCollectible>();
		collectible.OnCollectibleClaimed += LabReport_OnCollectibleClaimed;
		playerTransform = PlayerController.Instance.transform;
	}

	void Update() {
		if (!awaitingCollectible) {
			if (((Vector2)playerTransform.position - (Vector2)transform.position).magnitude < range) {
				if (tutInstance == null) {
					tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
					tutScript = tutInstance.GetComponent<ButtonTutorial>();
					tutScript.SetUp(intKey, gameObject);
				} else {
					tutScript.CancelFade();
				}
				// Start the Lab Report;
				if (Input.GetKeyDown(intKey)) {
					collectible.Collect();
					awaitingCollectible = true;
					if (tutInstance != null) {
						tutScript.Fade();
					}
				}
			} else if (tutInstance) {
				tutScript.Fade();
			}
		}
	}

	private void LabReport_OnCollectibleClaimed() {
		OnReportRead?.Invoke();
		auditor.GetComponent<Auditor>().updateLabReport();
		Destroy(this);
	}
}