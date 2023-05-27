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
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject spellsControls;
    [SerializeField] private GameObject polaroidOptions;
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
        if (polaroidMenu.activeSelf) TogglePolaroidMenu();
        SpellsPage();
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

    public void DisplayPolaroid(string polaroid)
    {
		currentMenu.SetActive(false);
        controller.AddCall(CollectibleController.CollectibleType.Polaroid, polaroid, false);
		awaitingCalls = true;
	}

	private void PauseMenu_OnCallsEnd() 
    {
        if (awaitingCalls) {
			currentMenu.SetActive(true);
			awaitingCalls = false;
		}
    }

    public void PolaroidPage() {
        spellsControls.SetActive(false);
        polaroidOptions.SetActive(true);
    }

    public void SpellsPage() {
        polaroidOptions.SetActive(false);
        spellsControls.SetActive(true);
    }
}
