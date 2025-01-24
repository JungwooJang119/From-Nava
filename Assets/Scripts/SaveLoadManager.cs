using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager instance;
    private static SaveSystem saveSystem;

    private void Awake() {
        if (instance == null) {
            instance = this;
            saveSystem = new SaveSystem();
        } else {
            // Destroy(this);
        }
    }

    void OnApplicationQuit() {
        AudioControl.Instance.Save();
		SaveSystem.SaveSerializablePlayerSettings();
    }
}
