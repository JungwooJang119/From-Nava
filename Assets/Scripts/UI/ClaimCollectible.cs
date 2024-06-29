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
        controller.OnCallsEnd += CollectibleCall_OnCallsEnd;
    }
    
    public void Collect() {
		awaitingCallback = true;
        foreach (ItemData data in collectibleCalls) controller.AddCall(data);
    }

    void CollectibleCall_OnCallsEnd() {
        if (awaitingCallback) OnCollectibleClaimed?.Invoke();
        awaitingCallback = false;
	}

    public void DoReward() {
        gameObject.SetActive(true);
    }
}
