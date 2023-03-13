using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.Escape))
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// Pause the game
    void Pause()
    {
        pauseMenuUI.SetActive(true);
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
    public void LoadPolaroids()
    {
        Debug.Log("Loading Polaroids...");
    }
}
