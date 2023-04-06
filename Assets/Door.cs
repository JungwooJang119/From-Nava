using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor() {
        animator.SetBool("IsUnlocked", true);
    }

    public void CloseDoor() {
        animator.SetBool("IsUnlocked", false);
    }
}
