using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PauseMenuOptions;

public class PauseMenuOptionWheelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private SectionType sectionToActivate;
    private float scaleMultiplier = 1.15f;
    private float scaleSpeed = 12f;

    private enum State {
        Idle,
        Normal,
        Hover,
        Underthrow,
        Overthrow,
        Disable
    } private State state = State.Idle;
    private bool interactable = true;

    private PauseMenuOptions masterScript;
    private Vector3 anchorScale;

    public event Action<PauseMenuOptionWheelButton> OnToggleEnd;
    private PauseMenuOptionWheelImage wheel;

    void Awake() {
        masterScript = GetComponentInParent<PauseMenuOptions>(true);
        anchorScale = transform.localScale;
        wheel = GetComponentInChildren<PauseMenuOptionWheelImage>(true);
        GetComponent<Button>().onClick.AddListener(OnClick);
        wheel.ToggleRotation(true);
    }

    void OnEnable() {
        if (sectionToActivate == SectionType.Audio) transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (interactable) {
            state = State.Hover;
        }
    }

    public void OnPointerExit(PointerEventData data) {
        if (interactable) {
            state = State.Normal;
        }
    }

    public void OnClick() {
        if (interactable) {
            interactable = false;
            state = State.Underthrow;
            masterScript.SwitchActiveSection(sectionToActivate);
        } if (sectionToActivate == SectionType.Audio) state = State.Disable;
    }

    void Update() {
        switch (state) {
            case State.Normal:
                if (ApproachScale(1f, scaleSpeed)) state = State.Idle;
                break;
            case State.Hover:
                ApproachScale(scaleMultiplier, scaleSpeed);
                break;
            case State.Underthrow:
                if (ApproachScale(0.95f, scaleSpeed)) state = State.Normal;
                break;
            case State.Overthrow:
                if (ApproachScale(scaleMultiplier * 1.05f, scaleSpeed)) {
                    state = State.Normal;
                    OnToggleEnd?.Invoke(this);
                } break;
            case State.Disable:
                if (ApproachScale(0f, scaleSpeed)) {
                    state = State.Idle;
                    OnToggleEnd?.Invoke(this);
                    gameObject.SetActive(false);
                } break;
        }
    }

    private bool ApproachScale(float multiplier, float speed) {
        if (transform.localScale != anchorScale * multiplier) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, anchorScale * multiplier, Time.unscaledDeltaTime * speed);
        } else return true;
        return false;
    }

    public bool Toggle(bool active) {
        if (active) {
            state = State.Overthrow;
            interactable = true;
        } else if (transform.localScale == Vector3.zero) {
            return true;
        } else state = State.Disable;
        return false;
    }

    public void Restore() {
        if (sectionToActivate == SectionType.Audio) {
            state = State.Idle;
            gameObject.SetActive(false);
        } else {
            interactable = true;
            state = State.Normal;
        }
    }
}
