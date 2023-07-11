using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PolaroidDataBank;

public class PauseMenuPolaroidButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private PolaroidType polaroidType;
    [SerializeField] private float scaleMultiplier = 1.05f;
    [SerializeField] private float scaleSpeed = 1.5f;
    private Vector3 anchorScale;

    private PauseMenuPolaroids masterScript;
    private PauseMenuPolaroidShowcase showScript;

    private enum State {
        Normal,
        HoverOverthrow,
        HoverUnderthrow,
        HoverNormal,
        OnClickUnderthrow,
        OnClickOverthrow,
    } private State state = State.Normal;

    void Awake() {
        anchorScale = transform.localScale;
        masterScript = GetComponentInParent<PauseMenuPolaroids>();
        showScript = masterScript.GetComponentInChildren<PauseMenuPolaroidShowcase>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnEnable() {
        state = State.Normal;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (state == State.Normal) {
            state = State.HoverOverthrow;
            showScript.Show(polaroidType);
        }
    }

    public void OnPointerExit(PointerEventData data) {
        if (state == State.HoverNormal
            || state == State.HoverOverthrow) {
            state = State.HoverUnderthrow;
            showScript.Hide();
        }
    }

    public void OnClick() {
        showScript.Expand();
        state = State.OnClickUnderthrow;
    }

    void Update() {
        switch (state) {
            case State.Normal:
                ApproachScaleByMultiplier(1f, 1f);
                break;
            case State.HoverOverthrow:
                if (ApproachScaleByMultiplier(scaleMultiplier * 1.05f, scaleSpeed)) state = State.HoverNormal;
                break;
            case State.HoverNormal:
                ApproachScaleByMultiplier(scaleMultiplier, scaleSpeed / 1.5f);
                break;
            case State.HoverUnderthrow:
                if (ApproachScaleByMultiplier(2f - scaleMultiplier, scaleSpeed)) state = State.Normal;
                break;
            case State.OnClickUnderthrow:
                if (ApproachScaleByMultiplier(2f - scaleMultiplier, scaleSpeed * 1.75f)) state = State.OnClickOverthrow;
                break;
            case State.OnClickOverthrow:
                if (ApproachScaleByMultiplier(scaleMultiplier * 1.1f, scaleSpeed * 2f)) state = State.Normal;
                break;
        }
    }

    private bool ApproachScaleByMultiplier(float multiplier, float speed) {
        if (transform.localScale != anchorScale * multiplier) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, anchorScale * multiplier, Time.unscaledDeltaTime * speed);
        } else return true;
        return false;
    }
}
