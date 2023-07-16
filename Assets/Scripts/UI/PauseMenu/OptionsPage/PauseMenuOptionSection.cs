using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PauseMenuOptions;

public class PauseMenuOptionSection : MonoBehaviour {

    public event Action<PauseMenuOptionSection> OnFadeEnd;

    [SerializeField] private SectionType sectionType;

    private float speed = 10f;
    private float currentAlpha;
    private float targetAlpha = 1;

    private Image[] images;
    private TextMeshProUGUI[] texts;

    void Awake() {
        images = GetComponentsInChildren<Image>(true);
        texts = GetComponentsInChildren<TextMeshProUGUI>(true);
    }

    private void OnEnable() {
        currentAlpha = 0;
        foreach (Image image in images) image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
        foreach (TextMeshProUGUI text in texts) text.alpha = currentAlpha;
    }

    void Update() {
        if (currentAlpha != targetAlpha) {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, speed * Time.unscaledDeltaTime);
            foreach (Image image in images) image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
            foreach (TextMeshProUGUI text in texts) text.alpha = currentAlpha;
            if (currentAlpha == targetAlpha) {
                OnFadeEnd?.Invoke(this);
                if (targetAlpha == 0) gameObject.SetActive(false);
            }
        }
    }

    public void FadeOut() {
        targetAlpha = 0;
    }

    public void FadeIn() {
        targetAlpha = 1;
    }

    public SectionType GetSectionType() {
        return sectionType;
    }

    public void Restore() {  
        if (sectionType == SectionType.Audio) {
            targetAlpha = 1;
        } else {
            currentAlpha = 0;
            targetAlpha = 0;
            foreach (Image image in images) image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
            foreach (TextMeshProUGUI text in texts) text.alpha = currentAlpha;
            gameObject.SetActive(false);
        }
    }
}