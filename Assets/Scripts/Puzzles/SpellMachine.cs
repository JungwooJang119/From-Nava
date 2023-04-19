using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMachine : MonoBehaviour
{
    [SerializeField] Spell spell;
	[SerializeField] SpellScriptObj spellData;
	[SerializeField] float timeBetweenCasts = 5;
	[SerializeField] bool active;
	[SerializeField] float time;
	[SerializeField] private string dir;

    void Update() {
        if (active) {
			if (time <= 0) {
				if (dir == "North") {
					spell.CastSpell(Vector2.up);
				} else {
					spell.CastSpell(Vector2.down);
				}
				var spellCasted = Instantiate(spell, transform.position, Quaternion.identity);
				spellCasted.transform.SetParent(transform);
				AudioControl.Instance.PlaySFX(spellData.sfxString, gameObject, 0.15f, 0.5f);
				time = timeBetweenCasts;
			}
			time -= Time.deltaTime;
		}
	}
	public void Activate() {
		active = true;
	}
	public void Deactivate() {
		active = false;
	}
}