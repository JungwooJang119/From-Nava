using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellCastManager : MonoBehaviour {
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

        PlayerController.Instance.animator.SetTrigger("doSpellCast");
        PlayerController.Instance.DeactivateMovement();
        StartCoroutine(CastSpell(spell, face));
    }

    IEnumerator CastSpell(Spell spell, Vector2 face) {
		AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
		yield return new WaitForSeconds(startLagTime);
        TriggerParticles(spell);
        Spell spellGO = Instantiate(spell, PlayerController.Instance.castPoint.position, Quaternion.identity);
        spellGO.CastSpell(PlayerController.Instance, face);
        PlayerController.Instance.ActivateMovement();
	}

    public void TriggerParticles(Spell s) {
        ParticleSystem ps = PlayerController.Instance.castPoint.GetComponent<ParticleSystem>();
        Color c = new Color(s.spell.particleColor[0], s.spell.particleColor[1], s.spell.particleColor[2], 1.0f);
        var col = ps.colorOverLifetime;
        col.enabled = true;

        Gradient grad = new Gradient();
        grad.SetKeys( new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(c, 1.0f) },
                                               new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 0.0f) } );
        col.color = grad;

        ps.Play();
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