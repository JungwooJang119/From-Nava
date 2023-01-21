using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptObj : ScriptableObject
{
    public float manaCost = 5f;
    public float speed = 15f;
    public float damageAmt = 10f;
    public float lifetime = 2f;
}
