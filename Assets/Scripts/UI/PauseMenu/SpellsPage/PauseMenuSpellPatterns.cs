using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSpellPatterns : MonoBehaviour, ISpellPage {

    [SerializeField] private Sprite disabledPattern;
    public Sprite DisabledPattern => disabledPattern;
}
