using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaMeter : MonoBehaviour
{
    [SerializeField] private Slider manaSlider;

    public void SetMaxMana(float mana) {
        manaSlider.maxValue = mana;
        manaSlider.value = mana;
    }

    public void SetManaMeter(float mana) {
        manaSlider.value = mana;
    }
}
