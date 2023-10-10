using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    public void ResetPosition() {
        pauseMenu.GetComponent<PauseMenu>().TogglePause();
        PlayerController.Instance.Damage(10);
    }
}
