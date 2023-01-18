using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptableObject : ScriptableObject
{
    public float magicCost = 5f;
    public float speed = 15f;
    public float damageAmt = 10f;
    public float spellRadius = 0.5f;
    public float spellLifetime = 2f;

    //Status effects
    //Thumbnail in UI
    //Time between casts each unique
    //Magic elements
}
