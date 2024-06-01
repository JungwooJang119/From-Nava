using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSpellPatterns : MonoBehaviour, ISpellPage {

    [SerializeField] private Sprite disabledPattern;
    [SerializeField] private Vector2 disabledDelta;
    public Sprite DisabledPattern => disabledPattern;
    public Vector2 DisabledDelta => disabledDelta;
}
