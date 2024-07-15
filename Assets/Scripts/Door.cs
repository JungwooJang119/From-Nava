using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Door : IInteractable, RewardObject
{
    protected Animator animator;
    private ClaimCollectible collectible;
    public bool isOpen;

    // 1) Gets the necessary changes in x and y for specific rotations of doros as necessary
    // 2) Makes doors listen for CollectibleClaimed so that it can reenable InteractBehavior
    // 3) Gets ClaimCollectible to get stuff
    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
        canTrigger = !isOpen;
        float rotation = transform.rotation.eulerAngles.z;
        switch (rotation) {
            case 0:
                xMod = .8f;
                break;
            case 180:
                xMod = -.8f;
                break;
            case 270:
                yMod = -1.8f;
                break;
        }
        collectible = collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += Door_OnCollectibleClaimed;
    }

    public virtual void OpenDoor() {
        if (isOpen) return;
        animator.SetBool("IsUnlocked", true);
        isOpen = true;
        AudioControl.Instance.PlaySFX("Door Opening", gameObject, 0.2f, 1f);
        canTrigger = false;
    }

    public void CloseDoor() {
        if (!isOpen) return;
        animator.SetBool("IsUnlocked", false);
        isOpen = false;
        AudioControl.Instance.PlaySFX("Door Opening", gameObject, 0.2f, 1f);
        canTrigger = true;
    }

    public void FlipDoor() {
        if (isOpen) {
            CloseDoor();
            isOpen = false;
        } else {
            OpenDoor();
            isOpen = true;
        }
    }

    protected override void InteractBehavior() {
        FadeButton();
        awaitingCollectible = true;
        collectible.Collect();
    }

    // When collectible claimed, wait a second and then recreate the button tutorial if necessary
    // KNOWN ISSUE: Scriptable objects are neccesary, or else when claiming the doors are gonna get weird.
    private void Door_OnCollectibleClaimed() {
        StartCoroutine(DelayedWakeup());
    }
    
    // Bug: If no Scriptable Object attached, the  
    private IEnumerator DelayedWakeup() {
        yield return new WaitForSeconds(.5f);
        CreateButtonTutorial();
        awaitingCollectible = false;
    }

    public void DoReward() {
        OpenDoor();
    }
}
