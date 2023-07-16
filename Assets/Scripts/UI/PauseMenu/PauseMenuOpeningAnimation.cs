using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOpeningAnimation : MonoBehaviour {

    [Header("It's very subtle. I know. I like it that way :D")]
    [Space()]
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float startingScale;
    private Vector3 anchorScale;

    void Awake() {
        anchorScale = transform.localScale;
    }

    void OnEnable() {
        transform.localScale = transform.localScale * startingScale;
    }

    void Update() {
        if (transform.localScale != anchorScale) transform.localScale = Vector3.MoveTowards(transform.localScale, anchorScale, scaleSpeed * Time.unscaledDeltaTime);
    }
}
