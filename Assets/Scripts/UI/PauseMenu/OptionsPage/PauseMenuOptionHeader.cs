using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PauseMenuOptions;

public class PauseMenuOptionHeader : MonoBehaviour {

    [Tooltip("Identifier for PauseMenuOptions;")]
    [SerializeField] private SectionType sectionType;

    [Tooltip("Position held when the Audio section is on;\nModified on the Transform (parent);")]
    [SerializeField] private Vector2 audioPosition;
    [Tooltip("Width of the Image Box when the Audio section is on;")]
    [SerializeField] private float audioWidth;
    [Tooltip("Position held when the Input section is on;\nModified on the Transform (parent);")]
    [SerializeField] private Vector2 inputPosition;
    [Tooltip("Width of the Image Box when the Input section is on;")]
    [SerializeField] private float inputWidth;

    public event Action<PauseMenuOptionHeader> OnMotionOver;

    private enum HeaderState {
        Idle,
        MovingY,
        MovingX,
        ExpandingWidth,
    } private HeaderState headerState = HeaderState.Idle;

    private float speed = 800f;
    private float targetY;
    private float targetX;
    private float targetWidth;

    private Transform parent;
    private RectTransform rect;

    void Awake() {
        parent = transform.parent;
        rect = GetComponentInChildren<Image>(true).rectTransform;
    }

    void Update() {
        switch (headerState) {
            case HeaderState.MovingY:
                if (parent.localPosition.y != targetY) {
                    var currentY = Mathf.MoveTowards(parent.localPosition.y, targetY, speed * 1.5f * Time.unscaledDeltaTime);
                    parent.localPosition = new Vector3(parent.localPosition.x, currentY, parent.localPosition.z);
                } else {
                    headerState = HeaderState.Idle;
                    if (sectionType == SectionType.Input) OnMotionOver?.Invoke(this);
                } break;
            case HeaderState.MovingX:
                if (parent.localPosition.x != targetX) {
                    var currentX = Mathf.MoveTowards(parent.localPosition.x, targetX, speed * 0.5f * Time.unscaledDeltaTime);
                    parent.localPosition = new Vector3(currentX, parent.localPosition.y, parent.localPosition.z);
                } else {
                    headerState = HeaderState.Idle;
                    if (sectionType == SectionType.Input) OnMotionOver?.Invoke(this);
                } break;
            case HeaderState.ExpandingWidth:
                if (rect.sizeDelta.x != targetWidth) {
                    var currentWidth = Mathf.MoveTowards(rect.sizeDelta.x, targetWidth, speed * Time.unscaledDeltaTime);
                    rect.sizeDelta = new Vector2(currentWidth, rect.sizeDelta.y);
                } else {
                    headerState = HeaderState.Idle;
                    if (sectionType == SectionType.Input) OnMotionOver?.Invoke(this);
                } break;
        }
    }

    public void RelocateY(SectionType sectionType) {
        headerState = HeaderState.MovingY;
        if (sectionType == SectionType.Audio) targetY = audioPosition.y;
        else if (sectionType == SectionType.Input) targetY = inputPosition.y;
    }

    public void RelocateX(SectionType sectionType) {
        headerState = HeaderState.MovingX;
        if (sectionType == SectionType.Audio) targetX = audioPosition.x;
        else if (sectionType == SectionType.Input) targetX = inputPosition.x;
    }

    public void ResizeWidth(SectionType sectionType) {
        headerState = HeaderState.ExpandingWidth;
        if (sectionType == SectionType.Audio) targetWidth = audioWidth;
        else if (sectionType == SectionType.Input) targetWidth = inputWidth;
    }

    public SectionType GetSectionType() {
        return sectionType;
    }

    public void Restore() {
        headerState = HeaderState.Idle;
        transform.parent.localPosition = audioPosition;
        var rect = GetComponentInChildren<Image>(true).rectTransform;
        rect.sizeDelta = new Vector2(audioWidth, rect.sizeDelta.y);
    }
}
