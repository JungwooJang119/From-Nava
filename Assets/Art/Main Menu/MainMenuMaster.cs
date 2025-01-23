using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using UnityEngine.EventSystems;

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
    [SerializeField] private TextMeshProUGUI[] _FileTexts; // File [] Text for New, Continue, Delete 
    [SerializeField] private TextMeshProUGUI[] _FileNameTexts; // Array of SaveProfile names in Play
    private Dictionary<MenuSection, MainMenuSection> menuDict;
    [SerializeField] private TMP_InputField field;
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
        // Transition is done within script as it's determined here if they should head to New or Continue
        // Set each text to 
        foreach (TextMeshProUGUI fileText in _FileTexts) {
            fileText.SetText($"File [{index + 1}]");
        }

        // Default as continue as new requires new logic
        MainMenuSection menuSection = menuDict[MenuSection.FileMenuContinue];
        if (SaveSystem.GetProfile(index) == null) {
            menuSection = menuDict[MenuSection.FileMenuNew];
            field.text = "";
            // ActivateNameInput();
        }
        ChangeActiveMenu(menuSection);
        ActivateNameInput();
    }

    private void ShowPreviousFile() {

    }

    // (Joseph 1 / 15 / 25) Attempt to create a new save file
    // For actually starting the game, it's done on the button continue, through the transition manager.
    // This function just sets up the profile. This is done to follow previous implementations.
    public void SetupNewProfile() {
        // The following checks for empty names does not work
        // Debug.Log(field.text);
        if (field.text.Length == 0) {
            return;
        }
        MainMenuSection menuSection = menuDict[MenuSection.FileMenuNew];
        name = field.text;
        SaveSystem.SetProfile(saveFileIndex, new SaveProfile(name));
        SaveSystem.SetCurrentProfile(saveFileIndex);
        // SaveSystem.SaveGame();
    }

    // (Joseph 1 / 15 / 25) Attempt to continue with the current SaveProfile
    // Similarly to SetupNewProfile, 
    public void ContinueWithProfile() {
        SaveSystem.LoadSaveProfile(saveFileIndex);
    }

    public void DeleteProfile() {
        SaveSystem.DeleteSaveProfile(saveFileIndex);
        _FileNameTexts[saveFileIndex].SetText("[Empty]");
    }

    public void ActivateNameInput(){
        // This is really buggy rn in that I need it to autoselect upon entering, but it takes a lot of clicks to register for some reason?
        // Not sure what's going on in the backend
        field.Select();
        // Debug.Log("Selected Triggered");
        // EventSystem.current.SetSelectedGameObject(field.gameObject);
        field.ActivateInputField();
        // StartCoroutine(InputName());
        // field.Select();
    }

    public IEnumerator InputName() {
        yield return new WaitForSeconds(1f);
        field.Select();
        field.ActivateInputField();
    }

    public void DebugSelect() {
        // Debug.Log("Debug Selected!");
    }
}
