using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PolaroidDataBank;
using static PauseMenuPolaroidShowcaseElement;

public class PauseMenuPolaroidShowcase : MonoBehaviour {

    private float scaleToScreenMultiplier = 1.5f;
    private float lerpValue = 0f;
    private float targetLerp = 0f;
    private float lerpSpeed = 2.5f;

    private enum State {
        Idle,
        ToScreen,
        OnScreen,
        ToPage
    } private State state = State.Idle;

    private Image polaroidImage;
    private TextMeshProUGUI polaroidText;
    private PauseMenuPolaroidShowcaseElement[] elements;
    private Dictionary<PolaroidType, PolaroidData> polaroidDict;
    private CanvasGroup polaroidFadeToBlack;

    void Awake() {
        elements = GetComponentsInChildren<PauseMenuPolaroidShowcaseElement>(true);
        foreach (PauseMenuPolaroidShowcaseElement element in elements) {
            if (element.GetElementType() == ElementType.Polaroid) polaroidImage = element.GetComponent<Image>();
            else if (element.GetElementType() == ElementType.Text) polaroidText = element.GetComponentInChildren<TextMeshProUGUI>();
        } polaroidDict = ReferenceSingleton.Instance.collectibleController.GetComponentInChildren<PolaroidDataBank>().GetPolaroidDict();
        polaroidFadeToBlack = GetComponentInChildren<CanvasGroup>();
    }

    void Update() {
        switch (state) {
            case State.ToScreen:
                AbortOnMouseInput();
                var resScreen = false;
                foreach (PauseMenuPolaroidShowcaseElement element in elements) {
                    resScreen = ApproachScaleAndLerp(element, scaleToScreenMultiplier, lerpSpeed);
                    ApproachPositionAndColor(element);
                } UpdateLerpFade(); 
                if (resScreen) state = State.OnScreen;
                break;
            case State.OnScreen:
                AbortOnMouseInput();
                break;
            case State.ToPage:
                var resPage = false;
                foreach (PauseMenuPolaroidShowcaseElement element in elements) {
                    resPage = ApproachScaleAndLerp(element, scaleToScreenMultiplier - 0.1f, lerpSpeed);
                    ApproachPositionAndColor(element);
                } UpdateLerpFade();
                if (resPage) {
                    state = State.Idle;
                    foreach (PauseMenuPolaroidShowcaseElement element in elements) element.UpdateData();
                } break;
        }
    }

    void OnDisable() {
        state = State.Idle;
        lerpValue = 0;
        if (elements[0].gameObject.activeSelf) {
            foreach (PauseMenuPolaroidShowcaseElement element in elements) {
                ApproachScaleAndLerp(element, 1f, 0f);
                ApproachPositionAndColor(element);
            } UpdateLerpFade();
        }
    }

    public void Show(PolaroidType polaroidType) {
        if (polaroidImage.gameObject.activeSelf) {
            foreach (PauseMenuPolaroidShowcaseElement element in elements) element.UpdateData();
            UpdatePolaroidData(polaroidType);
        } else {
            foreach (PauseMenuPolaroidShowcaseElement element in elements) element.gameObject.SetActive(true);
            UpdatePolaroidData(polaroidType);
        }
    }

    public void Hide() {
        foreach (PauseMenuPolaroidShowcaseElement element in elements) element.Hide();
    }

    public void Expand() {
        state = State.ToScreen;
        foreach (PauseMenuPolaroidShowcaseElement element in elements) element.OnClick();
        targetLerp = 1f;
    }

    private void UpdatePolaroidData(PolaroidType polaroidType) {
        var polaroidData = polaroidDict[polaroidType];
        polaroidImage.sprite = polaroidData.sprite;
        polaroidText.text = polaroidData.text.Replace("\\n", "\n"); // Unity is so beautiful, occassionally ;-;
    }

    private bool ApproachScaleAndLerp(PauseMenuPolaroidShowcaseElement element, float multiplier, float lerpSpeed) {
        if (lerpValue != targetLerp) {
            lerpValue = Mathf.MoveTowards(lerpValue, targetLerp, Time.unscaledDeltaTime * lerpSpeed);
            element.transform.localScale = Vector3.Lerp(element.AnchorScale, element.AnchorScale * multiplier, lerpValue);
        } else return true;
        return false;
    }

    private void ApproachPositionAndColor(PauseMenuPolaroidShowcaseElement element) {
        var rect = element.GetComponent<RectTransform>();
        var targetPosition = new Vector2(0, element.AnchorPosition.y * scaleToScreenMultiplier + 10f);
        rect.anchoredPosition = Vector2.Lerp(element.AnchorPosition, targetPosition, lerpValue);
        if (element.GetElementType() == ElementType.Text) {
            element.SetColor(Vector4.Lerp(element.AnchorTextColor, Color.white, lerpValue),
                             Vector4.Lerp(element.AnchorFillColor, Color.gray, lerpValue));
        }
    }

    private void AbortOnMouseInput() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
            state = State.ToPage;
            targetLerp = 0f;
        }
    }

    private void UpdateLerpFade() {
        if (lerpValue == 0) {
            if (polaroidFadeToBlack.blocksRaycasts) polaroidFadeToBlack.blocksRaycasts = false;
        } else if (!polaroidFadeToBlack.blocksRaycasts) polaroidFadeToBlack.blocksRaycasts = true;
        if (polaroidFadeToBlack.alpha != lerpValue / 2f) polaroidFadeToBlack.alpha = lerpValue / 2f;
    }
}
