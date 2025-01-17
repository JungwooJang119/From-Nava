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
    [SerializeField] private TextMeshProUGUI _FileText; // File [] Text for New
    [SerializeField] private TextMeshProUGUI _NameText; // NameInputText for New
    [SerializeField] private TextMeshProUGUI _PlayText; // File [] Text for Continue
    [SerializeField] private TextMeshProUGUI[] _FileNameTexts; // Array of SaveProfile names in Play
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
        // Debugging for Save Menu - Immediately adding a save profile for testing purposes
        // SaveSystem.SetProfile(0, new SaveProfile("Joseph"));

        // Done to grab all the save profiles
        for (int i = 0; i <= 2; i++) {
            if (SaveSystem.GetProfile(i) != null) {
                _FileNameTexts[i].SetText(SaveSystem.GetProfile(i).GetProfileName());
            } else {
                Debug.Log($"Profile {i + 1} is null!");
            }
            // _FileNameTexts[i].SetText(SaveSystem.GetProfile(i).GetProfileName());
        }

        // SaveSystem system = new SaveSystem();

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
        // In the instance that the selected profile does not have a save, we will create a new ave
        // Else go through the continue route
        if (SaveSystem.GetProfile(index) == null) {
            MainMenuSection menuSection = menuDict[MenuSection.FileMenuNew];
            ChangeActiveMenu(menuSection);
            _FileText.SetText($"File [{index + 1}]");
        } else {
            // Create appropriate logic for this function next time
            MainMenuSection menuSection = menuDict[MenuSection.FileMenuContinue];
            ChangeActiveMenu(menuSection);
            _PlayText.SetText($"File [{index + 1}]");
            ShowPreviousFile();
        }
    }

    private void ShowPreviousFile() {

    }

    // (Joseph 1 / 15 / 25) Attempt to create a new save file
    // For actually starting the game, it's done on the button continue, through the transition manager.
    // This function just sets up the profile. This is done to follow previous implementations.
    public void SetupNewProfile() {
        MainMenuSection menuSection = menuDict[MenuSection.FileMenuNew];
        name = _NameText.text;
        // Debug.Log($"Saving to {saveFileIndex}");
        SaveSystem.SetProfile(saveFileIndex, new SaveProfile(name));
        SaveSystem.SetCurrentProfile(saveFileIndex);
        // Debug.Log($"SAVED AS {name}"); // Comment out later
        SaveSystem.SaveGame();
    }

    // (Joseph 1 / 15 / 25) Attempt to continue with the current SaveProfile
    // Similarly to SetupNewProfile, 
    public void ContinueWithProfile() {
        // SerializableSaveProfile ssp = SaveSystem.LoadFromFile(SaveSystem.GetFilePath(saveFileIndex));
        SaveSystem.LoadSaveProfile(saveFileIndex);
    }

    public void DeleteProfile() {
        SaveSystem.DeleteSaveProfile(saveFileIndex);
        _FileNameTexts[saveFileIndex].SetText("[Empty]");
    }
}
