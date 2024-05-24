using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMachine : MonoBehaviour
{
    [SerializeField] GameObject spell;
	[SerializeField] SpellSO spellData;
	[SerializeField] float timeBetweenCasts = 5;
	[SerializeField] bool active;
	[SerializeField] float time;
	[SerializeField] private string dir;

    void Update() {
        if (active) {
			if (time <= 0) {
				var spellCasted = Instantiate(spell, transform.position, Quaternion.identity);
				spellCasted.transform.SetParent(transform);
				spellCasted.GetComponent<Spell>().direction = transform.eulerAngles.z < 180 ? Vector2.up : Vector2.down;
				spellCasted.transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 90);
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