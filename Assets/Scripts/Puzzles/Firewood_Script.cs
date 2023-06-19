using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewood_Script : MonoBehaviour
{
    public event Action<int> OnLitStatusChange;

	[SerializeField] private bool isLit;
    [SerializeField] private Material lineMaterial;
	[SerializeField] private GameObject[] adjFirewoods;

    private enum TriggerType {
        Fire,
        Ice,
        Wind,
    }

	private new GameObject light;
    private bool defaulLitStatus;
    private GameObject fire;

    private Firewood_Script[] firewoodScripts;
    private FirewoodLine[] firewoodLines;
    private float searchRange;

    void Start() {
        // Grab the Sprite Renderer of the object that has an animator, funky but works;
        fire = GetComponentInChildren<Animator>().gameObject;
        light = GetComponentInChildren<LightController>().gameObject;

		firewoodScripts = new Firewood_Script[adjFirewoods.Length];
        firewoodLines = new FirewoodLine[adjFirewoods.Length];
		for (int i = 0; i < adjFirewoods.Length; i++) {
			firewoodScripts[i] = adjFirewoods[i].GetComponent<Firewood_Script>();
            firewoodLines[i] = (new GameObject("Firewood Line " + i)).AddComponent<FirewoodLine>();
			firewoodLines[i].SetUpLine(transform, firewoodScripts[i].transform, lineMaterial);
		}

		defaulLitStatus = isLit;
		fire.SetActive(isLit);
		light.SetActive(isLit);
	}

    /*unfortunately i was unable to get the trigger colliders of fireball to work with a regular collider, so i've added another 
    circle collider and set it to trigger to make this work. Enemies aren't working so not sure what to do about that :/.
    The trigger attached to firewood will check for collisions by anything. If it's FIreball or Iceball, then it will light up
    or unlight.
    */
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<FireballBehavior>()) {
            SpellReaction(TriggerType.Fire);
            if (isLit) return;
        } else if (collision.gameObject.GetComponent<IceballBehavior>() && isLit) {
            SpellReaction(TriggerType.Ice);
        } else if (collision.gameObject.GetComponent<WindblastBehavior>()) {
			SpellReaction(TriggerType.Wind, collision.gameObject.GetComponent<Spell>().direction);
            return;
        } else {
			return;
        }
        ChangeLit();
        var litChange = isLit ? 1 : -1;
        foreach (Firewood_Script firewood in firewoodScripts) {
            litChange += !firewood.GetLit() ? 1 : -1;
        } OnLitStatusChange?.Invoke(litChange);
    }

    public void ChangeLit() {
        isLit = !isLit;
		fire.SetActive(true);
        fire.GetComponent<FirewoodFire>().Toggle(isLit);
	}

    public void SetDefaultLit() {
        if (isLit == defaulLitStatus) {
            return;
        } else {
            OnLitStatusChange?.Invoke(!isLit ? 1 : -1);
            ChangeLit();
        }
    }

	private void SpellReaction(TriggerType type, Vector2 direction = default(Vector2)) {
        foreach (FirewoodLine lineScript in firewoodLines) {
            // The following color codes are drawn from the spell sprites themselves;
			if (type == TriggerType.Fire && !isLit) {
                lineScript.gameObject.SetActive(true);
                var color1 = new Color32(223, 113, 38, 255);
                var color2 = new Color32(222, 54, 54, 255);
                lineScript.DrawLine(color1, color2, false);
			} else if (type == TriggerType.Ice) {
				lineScript.gameObject.SetActive(true);
				var color1 = new Color32(8, 100, 153, 255);
				var color2 = new Color32(128, 249, 255, 255);
				lineScript.DrawLine(color1, color2, false);
			}
        }
        if (isLit) {
            if (type == TriggerType.Fire) fire.GetComponent<FirewoodFire>().Toggle(true);
            else if (type == TriggerType.Wind) fire.GetComponent<FirewoodFire>().Twist(direction);
		}
    }

    public void AssignFirewoods(float range) {
        var colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
        var hits = new List<RaycastHit2D>();
        Physics2D.CircleCast(transform.position, range, Vector2.zero, new ContactFilter2D().NoFilter(), hits);
        var validGOs = new List<GameObject>();
        foreach (RaycastHit2D hitData in hits) {
            if (!colliders.Contains(hitData.collider) && hitData.collider.isTrigger) {
                validGOs.Add(hitData.transform.gameObject);
            }
        } adjFirewoods = validGOs.ToArray();
    }

    private void OnDrawGizmosSelected() {
        foreach (GameObject firewood in adjFirewoods) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, firewood.transform.position);
        }
    }

	public void SetLit(bool isLit) { this.isLit = isLit; }
	public bool GetLit() { return isLit; }
}