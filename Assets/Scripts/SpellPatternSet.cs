using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPatternSet : IInteractable, ISavable
{
    private ClaimCollectible collectible;
    [SerializeField] private string saveString;
    private bool wasCollected = false;


    // Start is called before the first frame update
    void Awake()
    {
        collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += ChestScript_OnCollectibleClaimed;
    }

    protected override void InteractBehavior() {
        StartCoroutine(AwaitCollectible());
        wasCollected = true;
        Save();
        FadeButton();
        Destroy(this.gameObject);
    }

    IEnumerator AwaitCollectible() {
		awaitingCollectible = true;
		collectible.Collect();
        var timer = 2.5f;
        while (awaitingCollectible || timer > 0) {
            if (timer > 0) timer -= Time.deltaTime;
            yield return null;
        }
        //Destroy(this.gameObject);
	}

    private void ChestScript_OnCollectibleClaimed() {
        awaitingCollectible = false;
    }

    public void Load(SaveProfile profile) {
        if (profile.GetBool(saveString, false)) {
            collectible = GetComponent<ClaimCollectible>();
            collectible.CollectSilent();
            Destroy(this.gameObject);
        }
        
    }

    public void Save() {
        SaveSystem.Current.SetBool(saveString, wasCollected);
    }
}
