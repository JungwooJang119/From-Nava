using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixture : MonoBehaviour
{
    [SerializeField] private GameObject light;
    public bool turnOff;
    // Start is called before the first frame update

    private void FixedUpdate() {
        if (turnOff) {
            ToggleLightOff();
        } else {
            ToggleLightOn();
        }
    }

    public void ToggleLightOn() {
        light.SetActive(true);
    }

    public void ToggleLightOff() {
        light.SetActive(false);
    }
}
