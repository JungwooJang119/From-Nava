using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    public bool isOpen;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
    }

    public void OpenDoor() {
        animator.SetBool("IsUnlocked", true);
        isOpen = true;
    }

    public void CloseDoor() {
        animator.SetBool("IsUnlocked", false);
        isOpen = false;
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
