using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modifies the default behavior of Iceball. Most spells follow the same pattern:
// Initialization, Lifetime, Death (just like your average cat), hence,
// some methods may be similar/identical to other spell's.

public class IceballBehavior : MonoBehaviour {
	[SerializeField] private SpellScriptObj spellData;	// Iceball spell data from the SO;

	// Simple state machine;
	private enum State {
		Start,
		Lifetime,
		End,
		Done,
	}
	private State state;

	private Spell spellScript;          // Reference for Spell.cs;
	private Transform castPoint;        // Transform the spell will be glued to until brrrrr;

	private float deathTimer;           // Timer to account for the spell lifetime;

	private float scaleFactor;          // Variables to control size;
	private SpriteRenderer[] sprRenderers; // Variable to control transparency;

	private ParticleSystem parSystem;   // Reference to the particle system (child);

    private Transform[] sprTransforms;  // Array of references to the transforms of children;

    // In today's news, start is called before the first frame update! O.O
    void Start() {
        // Suscribe to the OnDestroy event from Spell.cs
        spellScript = GetComponent<Spell>();
        spellScript.OnSpellDestroy += Spell_OnSpellDestroy;

        // Variables to manage the particle system;
        parSystem = GetComponentInChildren<ParticleSystem>();
		var ps = parSystem.emission;
		ps.enabled = false;

		// Variables to manage the scale and alpha;
		scaleFactor = transform.localScale.x;
		sprRenderers = GetComponentsInChildren<SpriteRenderer>();
		ChangeOpacity(-1f);

        // Variables to manage sprite rotation;
        sprTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            sprTransforms[i] = transform.GetChild(i);
        }

		// ALERT: Change if castPoint is no longer public in PlayerController.cs;
		castPoint = PlayerController.Instance.castPoint;
	}

	// Wait until the spell's life runs out. If it didn't collide, reduce the scale of the iceball sprites.
	// Then, yeet all the unnecesary components and set up a small particle burst for visual flavor.
	// The spell shall die when all particles run out ;-;
	void Update() {
		// Continously rotate sprites;
		RotateSprites(-0.75f, 0.5f, 5f);
		switch (state) {

			case State.Start:
				// Stay on cast point until release;
				transform.position = castPoint.position;
				// Deactivate spell on start;
				if (spellScript != null && spellScript.enabled) {
					ToggleComponents();
				}
				// Increase the opacity of the sprite;
				if (sprRenderers[0].color.a < 1f) {
					ChangeOpacity(Time.deltaTime * 7.5f);
				} else {
					// Enable emissions;
					var ps = parSystem.emission;
					ps.enabled = true;
					// Reactivate components;
					ToggleComponents();
					state = State.Lifetime;
				}
				break;

			case State.Lifetime:
				if (deathTimer < spellData.lifetime) {
					// Wait for lifetime;
					deathTimer += Time.deltaTime;
					break;
				}
				else {
					state = State.End;
				}
				break;

			case State.End:
				// Scale down the sprite;
				if (scaleFactor > 0) {
					ChangeSize(-15f);
				}
				else {
					// Stop spell and generate particles;
					if (spellScript != null) {
						CleanUp();
						GenerateBurst(1f, 10f);
					}
					else {
						GenerateBurst(0.75f, 15f);
					}
					state = State.Done;
				}
				break;
			case State.Done:
				// Stop emissions;
				parSystem.Stop();
				break;
		}
	}

	// Method to "disable" functionality at the start;
	private void ToggleComponents() {
		spellScript.enabled = spellScript.enabled ? false : true;
		var rb = GetComponent<Rigidbody2D>();
		rb.simulated = rb.simulated ? false : true;
		var coll = GetComponent<Collider2D>();
		coll.enabled = coll.enabled ? false : true;
	}

	// Method to take away functionality after the lifetime has been exhausted;
	private void CleanUp() {
        Destroy(spellScript);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
    }

    // Method called when the Spell script fires the destroy event! YEET!
    private void Spell_OnSpellDestroy(GameObject o) {
        deathTimer = spellData.lifetime;
        CleanUp();
    }

	// Method to change the size of the spell sprite;
	private void ChangeSize(float sizeMultiplier) {
		scaleFactor += Time.deltaTime * sizeMultiplier;
		transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
	}

	// Method to change the opacity of the spell sprite;
	private void ChangeOpacity(float alphaChange) {
		foreach (SpriteRenderer spR in sprRenderers) {
			var color = spR.color;
			color.a += alphaChange;
			spR.color = color;
		}
	}

	// Method to rotate the sprites;
	private void RotateSprites(float rotationRateOuter1, float rotationRateOuter2, float rotationRateInner) {
        int tranNumber = 0;
        foreach (Transform transform in sprTransforms) {
            if (tranNumber == 0) {
				transform.Rotate(0, 0, rotationRateInner);
				tranNumber = 1;
            } else if (tranNumber == 1) {
				transform.Rotate(0, 0, rotationRateOuter1);
				tranNumber = 2;
			} else {
				transform.Rotate(0, 0, rotationRateOuter2);
                tranNumber = 0;
			}
        }
    }

	// Method to generate a particle burst;
	private void GenerateBurst(float particleLifetime, float particleSpeed) {
        var parMainSystem = parSystem.main;
        parMainSystem.startLifetime = particleLifetime;
        parMainSystem.startSpeed = particleSpeed;
        parSystem.emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 60) });
        parSystem.Stop();
        parSystem.Play();
        Destroy(this.gameObject, parMainSystem.startLifetime.constant);
    }
}