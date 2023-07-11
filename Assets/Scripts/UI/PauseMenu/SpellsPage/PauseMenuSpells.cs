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

    void Awake() {
        pageTypeDict = new Dictionary<Type, GameObject>();
        var pages = GetComponentsInChildren<ISpellPage>(true);
        foreach (MonoBehaviour pageScript in pages) {
            pageTypeDict[pageScript.GetType()] = pageScript.gameObject;
        }
    }

    void OnEnable() {
        SetActiveSpellPageHome();
    }

    public void SetActiveSpellPage(Type type, TutorialDataBank.TutorialType tutorialType) {
        SetActiveSpellPage(type);
        activePage.GetComponent<PauseMenuSpellTutorials>().SetActiveTutorial(tutorialType);
    }

    public void SetActiveSpellPage(Type type) {
        foreach (KeyValuePair<Type, GameObject> pair in pageTypeDict) {
            if (pair.Key != type) pair.Value.SetActive(false);
        } activePage = pageTypeDict[type];
        activePage.SetActive(true);
    }

    public void SetActiveSpellPageHome() {
        SetActiveSpellPage(homePage.GetType());
    }
}

public interface ISpellPage { }
