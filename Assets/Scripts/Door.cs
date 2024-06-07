using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Door : IInteractable
{
    private Animator animator;

    public bool isOpen;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
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
    }

    public void OpenDoor() {
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
        Debug.Log("Door");
    }
}
