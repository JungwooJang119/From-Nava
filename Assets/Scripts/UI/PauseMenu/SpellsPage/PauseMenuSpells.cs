using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSpells : MonoBehaviour {

    // This dictionary identifies each subpage by the type of their master script;
    // It just works :V
    private Dictionary<Type, GameObject> pageTypeDict;
    [SerializeField] private MonoBehaviour homePage;
    private GameObject activePage;

    private PauseMenuPage masterScript;

    void Awake() {
        pageTypeDict = new Dictionary<Type, GameObject>();
        var pages = GetComponentsInChildren<ISpellPage>(true);
        foreach (MonoBehaviour pageScript in pages) {
            pageTypeDict[pageScript.GetType()] = pageScript.gameObject;
        } masterScript = GetComponentInParent<PauseMenuPage>();
    }

    void OnEnable() {
        SetActiveSpellPageHome();
    }

    public void SetActiveSpellPage(Type type, TutorialData tutorialType) {
        SetActiveSpellPage(type);
        StartCoroutine(WaitForActiveTutorial(tutorialType));
    }

    private IEnumerator WaitForActiveTutorial(TutorialData tutorialType) {
        while (!activePage.activeSelf) yield return null;
        activePage.GetComponent<PauseMenuSpellTutorials>().SetActiveTutorial(tutorialType);
    }

    public void SetActiveSpellPage(Type type, bool animate = true) {
        foreach (KeyValuePair<Type, GameObject> pair in pageTypeDict) {
            if (pair.Key != type) pair.Value.SetActive(false);
        } activePage = pageTypeDict[type]; 
        if (animate) {
            masterScript.Toggle(false, true);
            masterScript.OnFadeFinished += PauseMenuPage_OnFadeFinished;
        } else activePage.SetActive(true);
    }

    private void PauseMenuPage_OnFadeFinished(PauseMenuPage callingPage) {
        activePage.SetActive(true);
        masterScript.Toggle(true);
        masterScript.OnFadeFinished -= PauseMenuPage_OnFadeFinished;
    }

    public void SetActiveSpellPageHome(bool animate = false) {
        if (activePage != homePage.gameObject) SetActiveSpellPage(homePage.GetType(), animate);
    }
}

public interface ISpellPage { }
