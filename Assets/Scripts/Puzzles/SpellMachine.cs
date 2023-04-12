using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMachine : MonoBehaviour
{
    [SerializeField] Spell spell;
    [SerializeField] float timeBetweenCasts = 5;
	[SerializeField] bool active;
    private float time = 1;

    void Update() {
        if (active) {
			if (time <= 0) {
				spell.CastSpell(Vector2.down);
				Instantiate(spell, transform.position, Quaternion.identity);
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