using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PauseMenu;

public class PauseMenuPage : MonoBehaviour {

    [SerializeField] private MenuPage pageType;

    public MenuPage GetPageType() {
        return pageType;
    }
}
