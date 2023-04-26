using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private CollectibleController controller;
    private tranMode transition;
    private GameObject currentMenu;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    [SerializeField] private GameObject polaroidMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject reportMenu;
    [SerializeField] private GameObject notebook;
    private bool awaitingCalls;

    void Start() 
    {
        currentMenu = pauseMenuUI;
        controller = ReferenceSingleton.Instance.collectibleController;
        transition = ReferenceSingleton.Instance.transition;
		controller.OnCallsEnd += PauseMenu_OnCallsEnd;
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
        pauseMenuUI.SetActive(false);
		PlayerController.Instance.ActivateMovement();
		Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// Pause the game
    void Pause()
    {
        notebook.SetActive(false);
        pauseMenuUI.SetActive(true);
		PlayerController.Instance.DeactivateMovement();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// Quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    /// Load the options menu
    /// TODO: implement transitioning into scene for the options menu
    public void LoadOptions()
    {
        Debug.Log("Loading Options...");
    }

    /// Load the polaroids menu
    /// TODO: implement transitioning to the polaroids menu
    public void TogglePolaroidMenu()
    {
		if (currentMenu == pauseMenuUI) {
			currentMenu = polaroidMenu;
		} else {
			currentMenu = pauseMenuUI;
		}
		pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        polaroidMenu.SetActive(!polaroidMenu.activeSelf);
    }

    public void ToggleTutorialMenu()
    {
		if (currentMenu == pauseMenuUI) {
			currentMenu = tutorialMenu;
		} else {
			currentMenu = pauseMenuUI;
		}
		pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        tutorialMenu.SetActive(!tutorialMenu.activeSelf);
	}

    public void ToggleReportMenu()
    {
        if (currentMenu == pauseMenuUI) {
            currentMenu = reportMenu;
        } else {
            currentMenu = pauseMenuUI;
        }
		pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
		reportMenu.SetActive(!reportMenu.activeSelf);
    }

    public void DisplayPolaroid(string polaroid)
    {
		Time.timeScale = 1f;
		currentMenu.SetActive(false);
        controller.AddCall(CollectibleController.CollectibleType.Polaroid, polaroid, false);
		awaitingCalls = true;
	}

    public void DisplayTutorial(string tutorial)
    {
		Time.timeScale = 1f;
		currentMenu.SetActive(false);
		controller.AddCall(CollectibleController.CollectibleType.Tutorial, tutorial, false);
		awaitingCalls = true;
	}

    public void DisplayReport(string report)
    {
		Time.timeScale = 1f;
		currentMenu.SetActive(false);
		controller.AddCall(CollectibleController.CollectibleType.Report, report, false);
        awaitingCalls = true;
	}

	private void PauseMenu_OnCallsEnd() 
    {
        if (awaitingCalls) {
			currentMenu.SetActive(true);
			awaitingCalls = false;
		}
    }
}
