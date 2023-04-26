using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimCollectible : MonoBehaviour
{
    public event Action OnCollectibleClaimed;

    private CollectibleController controller;

    private bool awaitingCallback;

    [Serializable]
    private struct CollectibleCall {
		[Header("Type Key:                 0-Polaroid | 1-Tutorial | 2-Lab Report")]
		[Range(0, 2)]
        public int collectibleType;
        [Header("String identifier of the collectible to claim and display:")]
        public string name;
    }
    [Header("Calls to the Collectible Controller. Executed in order (first to last):")]
    [SerializeField] private CollectibleCall[] collectibleCalls;

    void Start() {
        controller = ReferenceSingleton.Instance.collectibleController;
        controller.OnCallsEnd += CollectibleCall_OnCallsEnd;
    }
    
    public void Collect() {
        foreach (CollectibleCall call in collectibleCalls) {
            if (call.collectibleType == 0) {
                controller.AddCall(CollectibleController.CollectibleType.Polaroid, call.name);
            } else if (call.collectibleType == 1) {
                controller.AddCall(CollectibleController.CollectibleType.Tutorial, call.name);
            } else {
                controller.AddCall(CollectibleController.CollectibleType.Report, call.name);
            }
        } awaitingCallback = true;
    }

    void CollectibleCall_OnCallsEnd() {
		if (awaitingCallback) OnCollectibleClaimed?.Invoke();
        awaitingCallback = false;
	}
}
