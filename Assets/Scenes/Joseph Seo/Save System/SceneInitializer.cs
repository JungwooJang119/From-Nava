using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    private void Awake() {
        SaveSystem.Current.Load();
    }
}
