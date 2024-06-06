using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the access to the Lab Report and the events shown On-Screen.

public class LabReport : IInteractable
{
	[SerializeField] static GameObject auditor;
	[SerializeField] private bool isCredits;

	// Room Control;
	public event Action OnReportRead;
	private ClaimCollectible collectible;	


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
        Destroy(this);
    }
}