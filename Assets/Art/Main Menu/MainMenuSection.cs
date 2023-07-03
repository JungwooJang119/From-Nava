using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuMaster;

public class MainMenuSection : MonoBehaviour {

    public event Action<bool> OnAlphaReached;

    [SerializeField] private MenuSection section;

    private Image alphaRef;
    private float sectionAlpha;
    private float transitionSpeed = 7.5f;
    private bool showSection;

    // Start is called before the first frame update
    void Awake() {
        SetAlphaInHierarchy(0);
        alphaRef = GetComponentInChildren<Image>(true);
    }

    private void OnEnable() {
        showSection = true;
    }

    // Update is called once per frame
    void Update() {
        var targetAlpha = showSection ? 1 : 0;
        sectionAlpha = Mathf.MoveTowards(sectionAlpha, targetAlpha, Time.deltaTime * transitionSpeed);
        if (alphaRef.color.a != targetAlpha) SetAlphaInHierarchy(sectionAlpha);
        else {
            OnAlphaReached?.Invoke(showSection);
            gameObject.SetActive(showSection);
        }
    }

    private void SetAlphaInHierarchy(float alpha) {
        var images = GetComponentsInChildren<Image>(true);
        foreach (Image image in images) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        } var texts = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI text in texts) {
            text.alpha = alpha;
        }
    }

    public void Toggle(bool active) {
        showSection = active;
    }

    public MenuSection GetSection() {
        return section;
    }
}
