using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPatternSet : IInteractable
{
    private ClaimCollectible collectible;


    // Start is called before the first frame update
    void Start()
    {
        collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += ChestScript_OnCollectibleClaimed;
    }

    protected override void InteractBehavior() {
        StartCoroutine(AwaitCollectible());
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
}
