using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the access to the Lab Report and the events shown On-Screen.

public class LabReport : IInteractable, ISavable
{
	[SerializeField] static GameObject auditor;
	[SerializeField] private bool isCredits;

	// Room Control;
	public event Action OnReportRead;
	private ClaimCollectible collectible;
	private bool reportRead = false;
	[SerializeField] private string saveString;	


	// Initialize variable references;
	void Start() {
		if (auditor == null) {
			auditor = GameObject.Find("Auditor");
		}
		collectible = GetComponent<ClaimCollectible>();
		collectible.OnCollectibleClaimed += LabReport_OnCollectibleClaimed;
	}

    protected override void InteractBehavior() {
        collectible.Collect();
        awaitingCollectible = true;
        FadeButton();
    }

    private void LabReport_OnCollectibleClaimed() {
		OnReportRead?.Invoke();
		if (!isCredits && GetComponent<AuditTarget>() != null) {
			auditor.GetComponent<Auditor>().updateLabReport();
		}
		reportRead = true;
		Save();
        Destroy(this);
    }

	public void Save() {
		SaveSystem.Current.SetCollectibleActive(saveString, gameObject.activeSelf);
		SaveSystem.Current.SetCollectibleCollected(saveString, reportRead);
	}

	public void Load(SaveProfile profile) {
		if (profile.GetCollectibleActive(saveString)) {
			gameObject.SetActive(true);
		}
		if (profile.GetCollectibleCollected(saveString)) {
			if (collectible == null) {
				collectible = GetComponent<ClaimCollectible>();
			} 
			collectible.CollectSilent();
			Destroy(this);
		}
	}
}