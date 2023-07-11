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
        foreach (PauseMenuPage page in childedMenus) {
            pageDict[page.GetPageType()] = page;
        }

        controller = ReferenceSingleton.Instance.collectibleController;
        transition = ReferenceSingleton.Instance.transition;
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape) && !controller.GetBusy())
        {
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
    public void Resume()
    {
        notebook.SetActive(true);
        ToggleActiveMenu(false);
        pauseMenuUI.SetActive(false);
		PlayerController.Instance.ActivateMovement();
		Time.timeScale = 1f;
        transition.DarkenIn(true);
        GameIsPaused = false;
    }

    /// Pause the game
    public void Pause()
    {
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
        foreach (KeyValuePair<MenuPage, PauseMenuPage> pair in pageDict) {
            if (pair.Value.GetPageType() != pageType) pair.Value.gameObject.SetActive(false);
        } activePage = pageType;
        ToggleActiveMenu(true);
        OnPageChanged?.Invoke(pageType);
    }

    private void ToggleActiveMenu(bool active) {
        pageDict[activePage].gameObject.SetActive(active);
    }

    public MenuPage GetActivePage() {
        return activePage;
    }

    /*

    /// Load the polaroids menu
    /// TODO: implement transitioning to the polaroids menu
    public void TogglePolaroidMenu() {
        if (currentMenu == pauseMenuUI) {
            currentMenu = polaroidMenu;
        } else {
            currentMenu = pauseMenuUI;
        }
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        polaroidMenu.SetActive(!polaroidMenu.activeSelf);
    }

    public void PolaroidPage() {
        spellsControls.SetActive(false);
        polaroidOptions.SetActive(true);
    }

    public void SpellsPage() {
        polaroidOptions.SetActive(false);
        spellsControls.SetActive(true);
    }

    /// Load the options menu
    /// TODO: implement transitioning into scene for the options menu
    public void LoadOptions()
    {
        Debug.Log("Loading Options...");
    }

    */
}
