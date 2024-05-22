using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMaster : MonoBehaviour {

    public enum MenuSection {
        Play,
        Buttons,
        Options,
        FileMenuContinue,
        FileMenuDelete,
        FileMenuNew
    }

    [SerializeField] private MainMenuSection activeSection;
    private Dictionary<MenuSection, MainMenuSection> menuDict;

    // Start is called before the first frame update
    void Start() {
        menuDict = new Dictionary<MenuSection, MainMenuSection>();
        var menuSections = GetComponentsInChildren<MainMenuSection>(true);
        foreach (MainMenuSection menuSection in menuSections) {
            menuDict[menuSection.GetSection()] = menuSection;
            menuSection.OnAlphaReached += MenuSection_OnAlphaReached;
        }
    }

    private void MenuSection_OnAlphaReached(bool active) {
        if (!active) activeSection.gameObject.SetActive(true);
    }

    public void ChangeActiveMenu(MainMenuSection sectionObject) {
        var menuSection = sectionObject.GetSection();
        foreach (KeyValuePair<MenuSection, MainMenuSection> sectionPair in menuDict) {
            if (sectionPair.Key != menuSection && sectionPair.Value.enabled) sectionPair.Value.Toggle(false);
        } activeSection = menuDict[menuSection];
    }
}
