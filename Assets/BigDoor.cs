using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor : Door
{
    [SerializeField] private GameObject exitSign;
    private GameObject signLight;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.SetBool("IsUnlocked", isOpen);
        // canTrigger = !isOpen;
        signLight = exitSign.transform.GetChild(0).gameObject;
    }

    public override void OpenDoor() {
        animator.SetBool("IsUnlocked", true);
        isOpen = true;
        canTrigger = false;
        AudioControl.Instance.PlaySFX("Door Opening", gameObject, 0.2f, 1f);
        signLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 3;
    }
}
