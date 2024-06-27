using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewoodListener : Listener {
    [SerializeField] private GameObject firewoodController;         // GameObject containing Firewood
    private Firewood[] firewoods;                                   // List of Firewood

    // GetComponentsInChildren will grab all Firewood in both the current game object and its children
    // Then the listener will subscribe to every change these firewood go through
    void Awake() {
        firewoods = firewoodController.GetComponentsInChildren<Firewood>();
        foreach (Firewood firewood in firewoods) {
            firewood.OnLitStatusChange += OnStatusChange;
        }
    }


    // When status has been changed, count all lit firewood, and if it equals the maximum possible amount this component is solved.
    protected override void OnStatusChange() {
        int firewoodCount = 0;
        foreach (Firewood firewood in firewoods) firewoodCount += firewood.IsPuzzleLit ? 1 : 0;
        status = firewoodCount == firewoods.Length;
        OnListen?.Invoke();
    }
}
