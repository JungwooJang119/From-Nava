using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptObj : ScriptableObject
{
    public float manaCost = 5f;
    public float speed = 15f;
    public int damageAmt = 10;
    public float lifetime = 2f;
    public string sfxString = "Fireball Cast";
    public float[] particleColor = new float [] {0.7607844f, 0.1411765f, 0.1058824f, 1.0f};
}