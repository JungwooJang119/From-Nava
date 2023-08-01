using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public event Action<MenuPage> OnPageChanged;

    public enum MenuPage {
        None,
        Spells,
        Polaroids,
        Options
    } private MenuPage activePage = MenuPage.Spells;
    private Dictionary<MenuPage, PauseMenuPage> pageDict;

    private CollectibleController controller;
    private tranMode transition;
    
    public GameObject pauseMenuUI;
    [SerializeField] private GameObject notebook;

    public static bool GameIsPaused = false;

    void Start() {
        pageDict = new Dictionary<MenuPage, PauseMenuPage>();
        var childedMenus = GetComponentsInChildren<PauseMenuPage>(true);
        foreach (PauseMenuPage page in childedMenus) pageDict[page.GetPageType()] = page;

        controller = ReferenceSingleton.Instance.collectibleController;
        transition = ReferenceSingleton.Instance.transition;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !controller.GetBusy()) {
            if (GameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    /// Resume the game
    public void Resume() {
        AudioControl.Instance.PlayVoidSFX("PMClosing", 0.25f);
        notebook.SetActive(true);
        ToggleActiveMenu(false);
        pauseMenuUI.SetActive(false);
		PlayerController.Instance.ActivateMovement();
		Time.timeScale = 1f;
        transition.DarkenIn(true);
        GameIsPaused = false;
    }

    /// Pause the game
    public void Pause() {
        AudioControl.Instance.PlayVoidSFX("PMOpening", 0.25f);
        notebook.SetActive(false);
        ToggleActiveMenu(true);
        pauseMenuUI.SetActive(true);
		PlayerController.Instance.DeactivateMovement();
        Time.timeScale = 0f;
        transition.DarkenOut(true);
        GameIsPaused = true;
    }

    /// Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeActivePage(MenuPage pageType) {
        AudioControl.Instance.PlayVoidSFX("PMPageChange" + UnityEngine.Random.Range(1, 3), 0.25f);
        foreach (KeyValuePair<MenuPage, PauseMenuPage> pair in pageDict) {
            if (pair.Value.GetPageType() != pageType) pair.Value.Toggle(false);
        } pageDict[activePage].OnFadeFinished += PauseMenuPage_OnFadeFinished;
        activePage = pageType;
        OnPageChanged?.Invoke(pageType);
    }

    private void PauseMenuPage_OnFadeFinished(PauseMenuPage callingPage) {
        ToggleActiveMenu(true);
        pageDict[activePage].Toggle(true);
        callingPage.OnFadeFinished -= PauseMenuPage_OnFadeFinished;
    }

    private void ToggleActiveMenu(bool active) {
        pageDict[activePage].gameObject.SetActive(active);
    }

    public MenuPage GetActivePage() {
        return activePage;
    }
}
