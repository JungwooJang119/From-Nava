using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    private void Start() {
        SaveSystem.Current.Load();
    }
}
