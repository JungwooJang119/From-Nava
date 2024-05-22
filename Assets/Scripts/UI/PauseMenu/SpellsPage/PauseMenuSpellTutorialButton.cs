using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuSpellTutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private float scaleMultiplier = 1.15f;
    [SerializeField] private float scaleSpeed = 2.5f;
    private Vector3 anchorScale;
    private bool hovered;

    private enum State {
        Normal,
        Hover,
        ClickedUnder,
        ClickedOver
    } private State state = State.Normal;

    private PauseMenuSpellTutorials pageScript;

    private enum ButtonType {
        Previous,
        Home,
        Next
    } [SerializeField] private ButtonType buttonType;
    private delegate bool TutorialButtonAction(bool set);
    private TutorialButtonAction buttonAction;
    private Button button;

    // Start is called before the first frame update
    void Awake() {
        anchorScale = transform.localScale;

        pageScript = GetComponentInParent<PauseMenuSpellTutorials>();
        switch (buttonType) {
            case ButtonType.Previous:
                buttonAction = (bool set) => { var res = CallPrevious(set);
                                               button.interactable = res;
                                               return res; };
                break;
            case ButtonType.Home:
                buttonAction = (bool returnForReal) => CallHome(returnForReal);
                break;
            case ButtonType.Next:
                buttonAction = (bool set) => { var res = CallNext(set);
                                               button.interactable = res;
                                               return res; };
                break;
        } button = GetComponent<Button>();
        button.onClick.AddListener(() => buttonAction.Invoke(true));
        button.onClick.AddListener(() => state = State.ClickedUnder);
        pageScript.OnTutorialChanged += PauseMenuSpellTutorials_OnTutorialChanged;
    }

    #region Button Actions

    public bool CallPrevious(bool set) {
        return pageScript.GetPreviousTutorial(set);
    }

    public bool CallHome(bool returnForReal) {
        if (returnForReal) GetComponentInParent<PauseMenuSpells>().SetActiveSpellPageHome(true);
        return returnForReal;
    }

    public bool CallNext(bool set) {
        return pageScript.GetNextTutorial(set);
    }

    #endregion

    void OnEnable() {
        transform.localScale = anchorScale;
        state = State.Normal;
    }

    private void PauseMenuSpellTutorials_OnTutorialChanged() {
        buttonAction.Invoke(false);
    }

    public void OnPointerEnter(PointerEventData data) {
        if (button.interactable) state = State.Hover;
        hovered = true;
    }

    public void OnPointerExit(PointerEventData data) {
        if (state == State.Hover) state = State.Normal;
        hovered = false;
    }

    // Update is called once per frame
    void Update() {
        if (!button.interactable) {
            if (state == State.Hover) state = State.Normal;
            hovered = false;
        }
 
        switch (state) {
            case State.Normal:
                if (transform.localScale != anchorScale) ScaleByMultiplier(1f, scaleSpeed);
                break;
            case State.Hover:
                if (transform.localScale != anchorScale * scaleMultiplier) ScaleByMultiplier(scaleMultiplier, scaleSpeed);
                break;
            case State.ClickedUnder:
                var underMultiplier = scaleMultiplier / 1.1f;
                if (transform.localScale != anchorScale * underMultiplier) ScaleByMultiplier(underMultiplier, scaleSpeed * 1.5f);
                else state = State.ClickedOver;
                break;
            case State.ClickedOver:
                var overMultiplier = scaleMultiplier * 1.1f;
                if (transform.localScale != anchorScale * overMultiplier) ScaleByMultiplier(overMultiplier, scaleSpeed * 2f);
                else state = hovered ? State.Hover : State.Normal;
                break;
        }
    }

    private void ScaleByMultiplier(float multiplier, float speed) {
        transform.localScale = Vector3.MoveTowards(transform.localScale, anchorScale * multiplier, Time.unscaledDeltaTime * speed);
    }
}
