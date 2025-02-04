using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private static GameSaver instance;
    private static SaveSystem saveSystem;

    private void Awake() {
        if (instance == null) {
            instance = this;
            saveSystem = new SaveSystem();
        } else {
            // Destroy();
        }

    }
}
