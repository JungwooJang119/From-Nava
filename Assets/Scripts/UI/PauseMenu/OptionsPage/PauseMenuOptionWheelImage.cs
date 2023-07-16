using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOptionWheelImage: MonoBehaviour {

    private float rotationSpeed = 2f;
    private bool rotate;

    void Update() {
        if (rotate) transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    public void ToggleRotation(bool active) {
        rotate = active;
    }
}
