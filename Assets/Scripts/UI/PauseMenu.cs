using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private TransitionManager transition;
    
    public GameObject pauseMenuUI;
    [SerializeField] private GameObject notebook;

    public static bool GameIsPaused = false;
    private bool notebookUnlocked = false;

    void Start() {
        pageDict = new Dictionary<MenuPage, PauseMenuPage>();
        var childedMenus = GetComponentsInChildren<PauseMenuPage>(true);
        foreach (PauseMenuPage page in childedMenus) pageDict[page.GetPageType()] = page;

        controller = ReferenceSingleton.Instance.collectibleController;
        transition = ReferenceSingleton.Instance.transition;
        controller.OnClaimCollectible += Controller_OnClaimCollectible;
        controller.OnCallsEnd += Controller_OnCallsEnd;
    }

    private void Controller_OnClaimCollectible(object sender, ItemCall call) {
        if (call.input.GetType() == typeof(TutorialData)) {
            TutorialData fireballData = (TutorialData) call.input;
            if (fireballData.name == "TutorialFireball") {
                notebookUnlocked = true;
            }
        }
    }
    private void Controller_OnCallsEnd() {
        if (notebookUnlocked) {
            notebook.SetActive(true);
            controller.OnClaimCollectible -= Controller_OnClaimCollectible;
            controller.OnCallsEnd -= Controller_OnCallsEnd;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !controller.IsBusy) {
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

    public void TogglePause() {
        if (!controller.IsBusy) {
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
        if (notebookUnlocked) {
            notebook.SetActive(true);
        }
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
        StartCoroutine(FinalFade(1f));
    }

    IEnumerator FinalFade(float finalFadeTime) {
        Resume();
        yield return new WaitForSeconds(transition.FadeOut(finalFadeTime));
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
