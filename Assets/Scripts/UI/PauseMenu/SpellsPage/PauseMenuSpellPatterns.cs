using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSpellPatterns : MonoBehaviour, ISpellPage {

    [SerializeField] private Sprite disabledPattern;
    private PauseMenuSpellPatternButton[] patternArr;

    void Awake() {
        patternArr = GetComponentsInChildren<PauseMenuSpellPatternButton>(true);
    }

    void OnEnable() {
        var tutorialsClaimed = ReferenceSingleton.Instance.collectibleController.GetTutorialList();
        foreach (PauseMenuSpellPatternButton button in patternArr) {
            button.Toggle(tutorialsClaimed.Contains(button.GetTutorialType().ToString()));
        }
    }

    public Sprite GetDisabledPattern() {
        return disabledPattern;
    }
}
