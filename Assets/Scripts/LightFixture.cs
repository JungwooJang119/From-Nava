using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixture : MonoBehaviour
{
    [SerializeField] private GameObject bruhLight;
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
        bruhLight.SetActive(true);
    }

    public void ToggleLightOff() {
        bruhLight.SetActive(false);
    }
}
