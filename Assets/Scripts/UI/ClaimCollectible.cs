using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimCollectible : MonoBehaviour, RewardObject {

    public event Action OnCollectibleClaimed;
    private CollectibleController controller;
    private bool awaitingCallback;

    [Header("Items to claim. Claimed in order (first to last):")]
    [SerializeReference] private ScriptableItem[] collectibleCalls;

    void Start() {
        controller = ReferenceSingleton.Instance.collectibleController;
    }
    
    public void Collect() {
		awaitingCallback = true;
        controller.OnCallsEnd += CollectibleCall_OnCallsEnd;
        foreach (ItemData data in collectibleCalls) controller.AddCall(data);
    }

    public void CollectSilent() {
        controller = ReferenceSingleton.Instance.collectibleController;
        // Debug.Log("Adding a call now!");
        // controller.AddSilentCall(collectibleCalls[0]);
        foreach (ItemData data in collectibleCalls) controller.AddSilentCall(data);
        // Debug.Log("Adding a call worked! Strange...");
    }

    void CollectibleCall_OnCallsEnd() {
        if (awaitingCallback) OnCollectibleClaimed?.Invoke();
        awaitingCallback = false;
        controller.OnCallsEnd -= CollectibleCall_OnCallsEnd;
	}

    public void DoReward() {
        gameObject.SetActive(true);
    }
}
