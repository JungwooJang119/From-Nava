using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static PauseMenu;

public class PauseMenuBookmark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [Tooltip("Menu page activated by this button;")]
    [SerializeField] private MenuPage pageType;
    [Tooltip("Offset distance the bookmark will move towards;")]
    [SerializeField] private float offsetDistance = 20f;
    [Tooltip("Direction towards which the bookmark will move;")]
    [Range(0, 360)]
    [SerializeField] private float directionAngle;
    [Tooltip("Speed at which the bookmark will move;")]
    [SerializeField] private float speed = 200f;
    [Tooltip("Selected scale multiplier;")]
    [SerializeField] private float selectedScaleMultiplier = 1.2f;
    private Vector3 selectedScale;

    private Image image;
    [SerializeField] [HideInInspector]
    private Color colorNormal, colorHighlight, colorPressed, colorSelected;
    private Color targetColor;
    private float colorSpeed = 1f;

    private enum State {
        Overthrow,
        Normalize,
        Underthrow,
        Anchor
    } private State state = State.Anchor;

    private RectTransform imageRect;
    private Vector2 offset;
    private Vector2 anchor;
    private Vector3 anchorScale;
    private bool selected;

    private PauseMenu menuScript;

    // Start is called before the first frame update
    void Awake() {
        imageRect = GetComponent<RectTransform>();
        anchor = imageRect.anchoredPosition;
        directionAngle *= Mathf.Deg2Rad;
        offset = new Vector2(offsetDistance * Mathf.Cos(directionAngle), offsetDistance * Mathf.Sin(directionAngle));
        anchorScale = transform.localScale;
        selectedScale = new Vector3(1, 1, 0) * selectedScaleMultiplier + new Vector3(0, 0, transform.localScale.z);
        image = GetComponent<Image>();
    }

    void OnEnable() {
        imageRect.anchoredPosition = anchor;
        state = State.Anchor;
    }

    void Start() {
        var parent = transform.parent;
        while (!parent.GetComponent<PauseMenu>()) {
            parent = parent.parent;
        } menuScript = parent.GetComponent<PauseMenu>();
        menuScript.OnPageChanged += PauseMenuBookmark_OnPageChanged;
        if (menuScript.GetActivePage() == pageType) selected = true;
        if (pageType != MenuPage.None) {
            var color = selected ? colorSelected : colorNormal;
            image.color = color;
            targetColor = color;
        }
    }

    public void OnPointerEnter(PointerEventData data) {
        state = State.Overthrow;
        if (!selected && targetColor != colorPressed) targetColor = colorHighlight;
    }

    public void OnPointerExit(PointerEventData data) {
        state = State.Underthrow;
        if (targetColor != colorPressed) targetColor = selected ? colorSelected : colorNormal;
    }

    public void OnPointerClick(PointerEventData data) {
        if (pageType != MenuPage.None) menuScript.ChangeActivePage(pageType);
    }

    private IEnumerator ButtonPressed() {
        targetColor = colorPressed;
        colorSpeed *= 1.5f;
        while (image.color != targetColor) yield return null;
        targetColor = selected ? colorSelected : colorNormal;
        while (image.color != targetColor) yield return null;
        colorSpeed /= 1.5f;
    }

    public void PauseMenuBookmark_OnPageChanged(MenuPage pageType) {
        if (selected) StartCoroutine(ButtonPressed());
        selected = pageType == this.pageType;
        
    }

    void Update() {
        switch (state) {
            case State.Overthrow:
                if (imageRect.anchoredPosition != new Vector2(anchor.x + offset.x * 2f,
                                                              anchor.y + offset.y * 2f)) {
                    imageRect.anchoredPosition = Vector2.MoveTowards(imageRect.anchoredPosition,
                                                                     new Vector2(anchor.x + offset.x * 2f,
                                                                                 anchor.y + offset.y * 2f), Time.unscaledDeltaTime * speed);
                } else state = State.Normalize;
                break;
            case State.Normalize:
                imageRect.anchoredPosition = Vector2.MoveTowards(imageRect.anchoredPosition,
                                                                 new Vector2(anchor.x + offset.x, anchor.y + offset.y), Time.unscaledDeltaTime * speed / 2.5f);
                break;
            case State.Underthrow:
                if (imageRect.anchoredPosition != new Vector2(anchor.x - offset.x * 0.8f,
                                                              anchor.y - offset.y * 0.8f)) {
                    imageRect.anchoredPosition = Vector2.MoveTowards(imageRect.anchoredPosition,
                                                                     new Vector2(anchor.x - offset.x * 0.8f,
                                                                                 anchor.y - offset.y * 0.8f), Time.unscaledDeltaTime * speed);
                } else state = State.Anchor;
                break;
            case State.Anchor:
                if (imageRect.anchoredPosition != anchor) {
                    imageRect.anchoredPosition = Vector2.MoveTowards(imageRect.anchoredPosition, anchor, Time.unscaledDeltaTime * speed / 2.5f);
                } break;
        } if (selected && transform.localScale != selectedScale) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, selectedScale, Time.unscaledDeltaTime);
        } else if (!selected && transform.localScale != anchorScale) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, anchorScale, Time.unscaledDeltaTime);
        } if (pageType != MenuPage.None && image.color != targetColor) image.color = Vector4.MoveTowards(image.color, targetColor, Time.unscaledDeltaTime * colorSpeed);
    }
}
