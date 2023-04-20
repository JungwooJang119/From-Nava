using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellCastManager : MonoBehaviour
{
    //Responsible for spawning spells

    [SerializeField] private float startLagTime;

    [Serializable]
    public struct EnumToSpell
    {
        public SpellType spellType;
        public Spell spell;
    }

    [SerializeField] private EnumToSpell[] enumToSpell;

    private Dictionary<SpellType, Spell> spellDict = new Dictionary<SpellType, Spell>();



    private void Awake() {
        foreach(EnumToSpell e in enumToSpell)
            spellDict[e.spellType] = e.spell;
    }

    private void OnEnable() {
        NotepadLogic.OnSpellCast += OnSpellCast;
    }

    private void OnDisable() {
        NotepadLogic.OnSpellCast -= OnSpellCast;
    }


    private void OnSpellCast(object sender, NotepadLogic.OnSpellCastArgs e)
    {
        
        if(e.spellType == SpellType.NONE)
        {
            return;
        }
        Vector2 face = new Vector2(PlayerController.Instance.FacingDir.x, PlayerController.Instance.FacingDir.y);
        Spell spell = spellDict[e.spellType];
        
        spell.CastSpell(face);
        PlayerController.Instance.animator.SetTrigger("doSpellCast");
        PlayerController.Instance.DeactivateMovement();
        StartCoroutine(CastSpell(spell));
    }

    IEnumerator CastSpell(Spell spell) {
		AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
		yield return new WaitForSeconds(startLagTime);
        Instantiate(spell, PlayerController.Instance.castPoint.position, Quaternion.identity);
        PlayerController.Instance.ActivateMovement();
	}

}


public enum SpellType {
    FIREBALL,
    CHAIR,
    ICEBALL,
    WINDBLAST,
    PIPLUP,
    NONE
}