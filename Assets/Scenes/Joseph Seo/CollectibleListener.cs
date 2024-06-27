using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleListener : Listener
{
    [SerializeField] private List<ClaimCollectible> collectibles;
    private int count = 0;
    void Awake() {
        foreach (ClaimCollectible obj in collectibles) obj.OnCollectibleClaimed += OnStatusChange;
    }

    // Whenever a collectible is read (LabReport, Copyable Spells) add to the count. Once this reaches the max, invoke and set this component to true.
    protected override void OnStatusChange() {
        count++;
        if (count == collectibles.Count) {
            status = true;
            OnListen?.Invoke();
        }
    }
}
