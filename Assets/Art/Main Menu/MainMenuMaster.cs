using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private int saveFileIndex;

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


    // (Joseph 1 / 15 / 25) Attempt at implementing the Save System
    public void SelectFile(int index) {
        saveFileIndex = index;
        // In the instance that the selected profile does not have a save, 
        if (SaveSystem.GetProfile(index) == null) {
            MainMenuSection menuSection = menuDict[MenuSection.FileMenuNew];
            ChangeActiveMenu(menuSection); // :(
            TextMeshProUGUI fileText = menuSection.transform.Find("FileText").GetComponent<TextMeshProUGUI>();
            fileText.SetText($"File [{index + 1}]");
        } else {
            MainMenuSection menuSection = menuDict[MenuSection.FileMenuContinue];
            ChangeActiveMenu(menuSection); 
        }
    }

    // (Joseph 1 / 15 / 25) Attempt to create a new save file
    public void StartNewGame() {
        // Set savefilename of profileindex to the appropriate one
        // start game
        SaveSystem.SetProfile(saveFileIndex, new SaveProfile("soreno"));
        Debug.Log("SAVED AS soreno");
    }

    public void ContinueGame() {

    }

    
}
