using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuOptionVolumeValue : MonoBehaviour {

    private enum SliderType {
        Master,
        Music,
        SFX
    } [SerializeField] private SliderType sliderType;

    private VolumeSliders sliderScript;
    private TextMeshProUGUI valueText;

    void Awake() {
        sliderScript = GetComponentInParent<VolumeSliders>();
        valueText = GetComponentInChildren<TextMeshProUGUI>();
        switch (sliderType) {
            case SliderType.Master:
                sliderScript.GetMasterSlider().onValueChanged.AddListener(UpdateDisplayValue);
                break;
            case SliderType.Music:
                sliderScript.GetMusicSlider().onValueChanged.AddListener(UpdateDisplayValue);
                break;
            case SliderType.SFX:
                sliderScript.GetSFXSlider().onValueChanged.AddListener(UpdateDisplayValue);
                break;
        }
    }

    private void UpdateDisplayValue(float value) {
        value = (int) (value * 10) / 10f;
        valueText.text = value.ToString("F1");
    }
}
