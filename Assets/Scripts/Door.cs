using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Door : MonoBehaviour
{
    private Animator animator;

    public bool isOpen;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
    }

    public void OpenDoor() {
        if (isOpen) return;
        animator.SetBool("IsUnlocked", true);
        isOpen = true;
        AudioControl.Instance.PlaySFX("Door Opening", gameObject, 0.2f, 1f);
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
}
