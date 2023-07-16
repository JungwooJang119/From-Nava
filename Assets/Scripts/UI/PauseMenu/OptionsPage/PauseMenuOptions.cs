using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuOptions : MonoBehaviour {

    public enum SectionType {
        Audio,
        Input
    } private Dictionary<SectionType, PauseMenuOptionSection> sectionDict;
    private Dictionary<SectionType, PauseMenuOptionHeader> headerDict;
    private Dictionary<SectionType, PauseMenuOptionWheelButton> wheelDict;
    private SectionType activeSection;

    private enum TransitionState {
        Idle,
        MoveHeaderY,
        FadeInSection,
        ToggleWheelOut,
        MoveHeadersX,
        ResizeHeaders,
        ToggleWheelIn
    } private TransitionState transitionState;

    private LinkedList<TransitionState> transitionSequence;
    private LinkedListNode<TransitionState> headState;

    private bool nodesSwapped = false;

    void Awake() {
        sectionDict = new Dictionary<SectionType, PauseMenuOptionSection>();
        var sections = GetComponentsInChildren<PauseMenuOptionSection>(true);
        foreach (PauseMenuOptionSection section in sections) {
            sectionDict[section.GetSectionType()] = section;
        }
        headerDict = new Dictionary<SectionType, PauseMenuOptionHeader>();
        wheelDict = new Dictionary<SectionType, PauseMenuOptionWheelButton>();
        var headers = GetComponentsInChildren<PauseMenuOptionHeader>(true);
        foreach (PauseMenuOptionHeader header in headers) {
            var sectionType = header.GetSectionType();
            headerDict[sectionType] = header;
            wheelDict[sectionType] = header.transform.parent.GetComponentInChildren<PauseMenuOptionWheelButton>(true);
        }
    }

    private void OnEnable() {
        Reset();
    }

    private void Update() {
        switch (transitionState) {
            case TransitionState.MoveHeaderY:
                var movingHeader = headerDict[SectionType.Input];
                movingHeader.RelocateY(activeSection);
                movingHeader.OnMotionOver += PauseMenuOptionHeader_OnMotionOver;
                transitionState = TransitionState.Idle;
                break;
            case TransitionState.FadeInSection:
                sectionDict[activeSection].gameObject.SetActive(true);
                sectionDict[activeSection].FadeIn();
                if (AdvanceTransitionState()) transitionState = headState.Value;
                break;
            case TransitionState.ToggleWheelOut:
                foreach (KeyValuePair<SectionType, PauseMenuOptionWheelButton> wheelPair in wheelDict) {
                    if (wheelPair.Key == activeSection) {
                        wheelPair.Value.gameObject.SetActive(true);
                        if (wheelPair.Value.Toggle(false) && AdvanceTransitionState()) transitionState = headState.Value;
                        else wheelPair.Value.OnToggleEnd += PauseMenuOptionsWheelButton_OnToggleEnd;
                        break;
                    } transitionState = TransitionState.Idle;
                } break;
            case TransitionState.MoveHeadersX:
                foreach (KeyValuePair<SectionType, PauseMenuOptionHeader> headerPair in headerDict) {
                    headerPair.Value.RelocateX(activeSection);
                    headerPair.Value.OnMotionOver += PauseMenuOptionHeader_OnMotionOver;
                } transitionState = TransitionState.Idle;
                if (nodesSwapped) SwapNodes();
                break;
            case TransitionState.ResizeHeaders:
                foreach (KeyValuePair<SectionType, PauseMenuOptionHeader> headerPair in headerDict) {
                    headerPair.Value.ResizeWidth(activeSection);
                    headerPair.Value.OnMotionOver += PauseMenuOptionHeader_OnMotionOver;
                } transitionState = TransitionState.Idle;
                if (!nodesSwapped) SwapNodes();
                break;
            case TransitionState.ToggleWheelIn:
                foreach (KeyValuePair<SectionType, PauseMenuOptionWheelButton> wheelPair in wheelDict) {
                    if (wheelPair.Key != activeSection) {
                        wheelPair.Value.gameObject.SetActive(true);
                        wheelPair.Value.Toggle(true);
                        AdvanceTransitionState();
                        break;
                    }
                } transitionState = TransitionState.Idle;
                break;
        }
    }

    private void SwapNodes() {
        var prev = headState.Value;
        headState.Value = headState.Previous.Value;
        headState.Previous.Value = prev;
        nodesSwapped = !nodesSwapped;
    }

    private void PauseMenuOptionSection_OnFadeEnd(PauseMenuOptionSection callingSection) {
        transitionState = TransitionState.MoveHeaderY;
        callingSection.OnFadeEnd -= PauseMenuOptionSection_OnFadeEnd;
    }

    private void PauseMenuOptionsWheelButton_OnToggleEnd(PauseMenuOptionWheelButton invoker) {
        if (AdvanceTransitionState()) transitionState = headState.Value;
        invoker.OnToggleEnd -= PauseMenuOptionsWheelButton_OnToggleEnd;
    }

    private void PauseMenuOptionHeader_OnMotionOver(PauseMenuOptionHeader invoker) {
        if (AdvanceTransitionState()) transitionState = headState.Value;
        invoker.OnMotionOver -= PauseMenuOptionHeader_OnMotionOver;
    }

    // Returns true if the sequence had another state to jump to, false if the sequence reset;
    public bool AdvanceTransitionState() {
        if (headState.Next != null) { headState = headState.Next; return true; }
        else { headState = transitionSequence.First.Next; return false; }
    }
    
    public void SwitchActiveSection(SectionType type) {
        var previousSection = sectionDict[activeSection];
        previousSection.FadeOut();
        previousSection.OnFadeEnd += PauseMenuOptionSection_OnFadeEnd;
        activeSection = type;
    }

    private void Reset() {
        transitionState = TransitionState.Idle;
        activeSection = SectionType.Audio;
        BuildLinkedList();
        foreach (KeyValuePair<SectionType, PauseMenuOptionSection> section in sectionDict) {
            section.Value.gameObject.SetActive(true);
            section.Value.Restore();
            section.Value.OnFadeEnd -= PauseMenuOptionSection_OnFadeEnd;
        } foreach (KeyValuePair<SectionType, PauseMenuOptionHeader> header in headerDict) {
            header.Value.Restore();
            header.Value.OnMotionOver -= PauseMenuOptionHeader_OnMotionOver;
        } foreach (KeyValuePair<SectionType, PauseMenuOptionWheelButton> wheel in wheelDict) {
            wheel.Value.gameObject.SetActive(true);
            wheel.Value.Restore();
            wheel.Value.OnToggleEnd -= PauseMenuOptionsWheelButton_OnToggleEnd;
        }
    }

    private void BuildLinkedList() {
        transitionSequence = new LinkedList<TransitionState>();
        var states = Enum.GetValues(typeof(TransitionState));
        foreach (TransitionState state in states) transitionSequence.AddLast(state);
        headState = transitionSequence.First.Next;
        nodesSwapped = false;
    }
}
