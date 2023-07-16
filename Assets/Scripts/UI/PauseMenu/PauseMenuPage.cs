using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using static PauseMenu;

public class PauseMenuPage : MonoBehaviour {

    public event Action<PauseMenuPage> OnFadeFinished;

    [SerializeField] private MenuPage pageType;

    private Dictionary<Image, float> imageAlphaDict;
    private Dictionary<TextMeshProUGUI, float> textAlphaDict;
    private Dictionary<VideoPlayer, float> videoAlphaDict;

    private float lerpValue = 1f;
    private float lerpSpeed = 12f;
    private bool stayActive;

    private enum State {
        FadeIn,
        Idle,
        FadeOut
    } private State state = State.Idle;

    private void Awake() {
        imageAlphaDict = new Dictionary<Image, float>();
        var images = GetComponentsInChildren<Image>(true);
        foreach (Image image in images) imageAlphaDict[image] = image.color.a;
        textAlphaDict = new Dictionary<TextMeshProUGUI, float>();
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts) textAlphaDict[text] = text.alpha;
        videoAlphaDict = new Dictionary<VideoPlayer, float>();
        var videos = GetComponentsInChildren<VideoPlayer>();
        foreach (VideoPlayer video in videos) videoAlphaDict[video] = video.targetCameraAlpha;
    }

    private void OnEnable() {
        if (state == State.Idle) state = State.FadeIn;
    }

    private void Update() {
        switch (state) {
            case State.FadeIn:
                if (Fade(true)) state = State.Idle;
                break;
            case State.FadeOut:
                if (Fade(false)) {
                    if (!stayActive) gameObject.SetActive(false);
                    else stayActive = false;
                    OnFadeFinished?.Invoke(this);
                } break;
        }
    }

    public void Toggle(bool active, bool stayActive = false) {
        state = active ? State.FadeIn : State.FadeOut;
        this.stayActive = stayActive;
    }

    private bool Fade(bool active) {
        lerpValue = Mathf.MoveTowards(lerpValue, active ? 1 : 0, Time.unscaledDeltaTime * lerpSpeed);
        foreach (KeyValuePair<Image, float> image in imageAlphaDict) {
            image.Key.color = new Color(image.Key.color.r, image.Key.color.g, image.Key.color.b, Mathf.Lerp(0, image.Value, lerpValue));
        } foreach (KeyValuePair<TextMeshProUGUI, float> text in textAlphaDict) {
            text.Key.alpha = Mathf.Lerp(0, text.Value, lerpValue);
        } foreach (KeyValuePair<VideoPlayer, float> video in videoAlphaDict) {
            video.Key.targetCameraAlpha = Mathf.Lerp(0, video.Value, lerpValue);
        } if (lerpValue == (active ? 1 : 0)) return true;
        else return false;
    }

    // Return value specifies if the page was loaded instantaniously; 
    public bool UpdateDictionary(Image image, float alpha) {
        if (imageAlphaDict != null) {
            imageAlphaDict[image] = alpha; return false;
        } else return true;
    }

    // Return value specifies if the page was loaded instantaniously; 
    public bool UpdateDictionary(TextMeshProUGUI text, float alpha) {
        if (textAlphaDict != null) {
            textAlphaDict[text] = alpha; return false;
        } else return true;
    }

    public MenuPage GetPageType() {
        return pageType;
    }
}
