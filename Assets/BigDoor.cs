using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor : Door
{
    private Animator animator;

    [SerializeField] private GameObject exitSign;
    private GameObject signLight;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
        signLight = exitSign.transform.GetChild(0).gameObject;
    }

    public override void OpenDoor() {
        animator.SetBool("IsUnlocked", true);
        isOpen = true;
        AudioControl.Instance.PlaySFX("Door Opening", gameObject, 0.2f, 1f);
        signLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 3;
    }
}
