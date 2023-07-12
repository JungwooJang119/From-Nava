using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuPolaroidShowcaseElement : MonoBehaviour {

    private float scaleMultiplier = 1.05f;
    private float lerpSpeed = 1f;

    private float lerpValue;
    public Vector3 AnchorScale { get; set; }
    public Vector2 AnchorPosition { get; set; }
    public Color AnchorTextColor { get; set; }
    public Color AnchorFillColor { get; set; }

    public enum ElementType {
        Polaroid,
        Text
    } [SerializeField] private ElementType elementType;

    private enum State {
        Idle,
        Show,
        Underthrow,
        Normal,
        Hide
    } private State state = State.Idle;

    private Image[] images;
    private TextMeshProUGUI[] texts;

    void Awake() {
        AnchorScale = transform.localScale;
        AnchorPosition = GetComponent<RectTransform>().anchoredPosition;
        AnchorTextColor = GetComponentInChildren<TextMeshProUGUI>() ? GetComponentInChildren<TextMeshProUGUI>().color : Color.white;
        AnchorFillColor = GetComponent<Image>().color;
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    void OnEnable() {
        if (state == State.Idle) {
            state = State.Show;
        } else {
            state = State.Idle;
            gameObject.SetActive(false);
        }
    }

    void Update() {
        switch (state) {
            case State.Show:
                if (ApproachScaleAndAlpha(scaleMultiplier, 1f, lerpSpeed * 10f)) {
                    state = State.Underthrow;
                } break;
            case State.Underthrow:
                if (ApproachScaleByMultiplier(0.95f, lerpSpeed)) state = State.Normal;
                break;
            case State.Normal:
                if (ApproachScaleByMultiplier(1f, 1f)) state = State.Idle;
                break;
            case State.Hide:
                if (ApproachScaleAndAlpha(scaleMultiplier, 0f, lerpSpeed * 1.5f)) {
                    state = State.Idle;
                    gameObject.SetActive(false);
                } break;
        }
    }

    private bool ApproachScaleAndAlpha(float multiplier, float targetLerp, float lerpSpeed) {
        if (lerpValue != targetLerp) {
            lerpValue = Mathf.MoveTowards(lerpValue, targetLerp, Time.unscaledDeltaTime * lerpSpeed);
            transform.localScale = Vector3.Lerp(AnchorScale, AnchorScale * multiplier, targetLerp == 0 ? 1 - lerpValue : lerpValue);
            foreach (Image image in images) image.color = new Color(image.color.r, image.color.g, image.color.b, lerpValue);
            foreach (TextMeshProUGUI text in texts) text.alpha = lerpValue;
        } else return true;
        return false;
    }

    public void SetColor(Color textColor, Color fillColor) {
        foreach (Image image in images) {
            if (image == GetComponent<Image>()) {
                image.color = fillColor;
            } else image.color = textColor;
        } foreach (TextMeshProUGUI text in texts) text.color = textColor;
    }

    private bool ApproachScaleByMultiplier(float multiplier, float speed) {
        if (transform.localScale != AnchorScale * multiplier) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, AnchorScale * multiplier, Time.unscaledDeltaTime * speed);
        } else return true;
        return false;
    }

    public void OnClick() {
        state = State.Idle;
    }

    public void UpdateData() {
        state = State.Show;
    }

    public void Hide() {
        state = State.Hide;
    }

    public ElementType GetElementType() {
        return elementType;
    }
}
