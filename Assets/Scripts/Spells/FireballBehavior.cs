using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modifies the default behavior of Fireball. Most spells follow the same pattern:
// Initialization, Lifetime, Death (just like your average human being), hence,
// some methods may be similar/identical to other spell's.

public class FireballBehavior : MonoBehaviour {
    [SerializeField] private SpellScriptObj spellData;  // Fireball spell data from the SO;

    // Simple state machine;
    private enum State {
        Start,
        Lifetime,
        End,
        Done,
    } private State state;

    private Spell spellScript;          // Reference for Spell.cs;
	private Transform castPoint;        // Transform the spell will be glued to until brrrrr;

	private float deathTimer;           // Timer to account for the spell lifetime;

    private float scaleFactor;          // Variables to control size;
    private float maxScaleFactor;

    private ParticleSystem parSystem;   // Reference to the particle system (child);

    private UnityEngine.Rendering.Universal.Light2D[] lightList; // Array to store light references;
    private float[] lightBounds;        // Target outer radii of light sources;
    private float lightRate = 0.375f;   // Rate at which the light will change;

    // Did you know start is called before the first frame update? :V
    void Start() {
        // Suscribe to the OnDestroy event from Spell.cs;
        spellScript = GetComponent<Spell>();
        spellScript.OnSpellDestroy += Spell_OnSpellDestroy;
        
        // Variables to manage the particle system;
        parSystem = GetComponentInChildren<ParticleSystem>();
        var ps = parSystem.emission;
        ps.enabled = false;

        // Variables to manage the scale;
        maxScaleFactor = transform.localScale.x;
        scaleFactor = 0;
        ChangeSize(0f);

        // Variables to manage the lights;
        lightList = GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>();
        lightBounds = new float[lightList.Length];
        for (int i = 0; i < lightList.Length; i++) {
			lightBounds[i] = lightList[i].pointLightOuterRadius;
            lightList[i].pointLightOuterRadius = 0;
		}

		// ALERT: Change if castPoint is no longer public in PlayerController.cs;
		castPoint = PlayerController.Instance.castPoint;
	}

    // Wait until the spell's life runs out. If it didn't collide, reduce the scale of the fireball sprite
    // and reduce the radius of the light source to 0. Then, yeet all the unnecesary components and set up a
    // small particle burst for visual flavor. The spell shall die when all particles run out ;-;
    void Update() {
        switch (state) {

            case State.Start:
                // Stay on cast point until release;
                transform.position = castPoint.position;
                // Deactivate spell on start;
				if (spellScript != null && spellScript.enabled) {
					ToggleComponents();
				}
                // Expand light source;
                ModifyLight(lightRate);
                // Scale up the sprite;
				if (scaleFactor < maxScaleFactor) {
					ChangeSize(7.5f);
				} else {
                    // Enable emissions;
					var ps = parSystem.emission;
					ps.enabled = true;
					// Reactivate components;
					if (spellScript != null) ToggleComponents();
                    state = State.Lifetime;
				}
                break;

            case State.Lifetime:
				if (deathTimer < spellData.lifetime) {
                    // Wait for lifetime;
					deathTimer += Time.deltaTime;
                    // Continue expanding light source;
                    ModifyLight(lightRate);
					// Small scale oscillation effect. Too lazy to make it cleaner :(
					scaleFactor = Mathf.Sin(Time.time * 7.5f) * maxScaleFactor / 20f + maxScaleFactor;
					transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
					break;
				} else {
					// Reset radii bounds for light source;
					for (int i = 0; i < lightBounds.Length; i++) { lightBounds[i] = 0; }
					state = State.End;
				}
                break;

            case State.End:
                // Dim the light sources;
				ModifyLight(-lightRate);
                // Scale down the sprite;
				if (scaleFactor > 0) {
					ChangeSize(-10f);
				} else {
                    // Stop spell and generate particles;
					if (spellScript != null) {
						CleanUp();
						GenerateBurst(1f, 100f);
					} else {
						GenerateBurst(0.75f, 150f);
					}
					state = State.Done;
				}
                break;
            case State.Done:
                // Stop emissions;
				parSystem.Stop();
                // Continue dimming the light sources;
				ModifyLight(-lightRate);
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
		AudioControl.Instance.PlaySFX("Fireball Collision", gameObject, 0.2f);
		deathTimer = spellData.lifetime;
        CleanUp();
    }

    // Method to change the size of the spell sprite;
    private void ChangeSize(float sizeMultiplier) {
        scaleFactor += Time.deltaTime * sizeMultiplier;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

	// Method to modify the light sources attached to the spell;
	private void ModifyLight(float rate) {
        for (int i = 0; i < lightList.Length; i++) {
            if (lightBounds[0] > 0) {
				lightList[i].pointLightOuterRadius = Mathf.Min(lightBounds[i], lightList[i].pointLightOuterRadius + rate);
			} else {
				lightList[i].pointLightOuterRadius = Mathf.Max(lightBounds[i], lightList[i].pointLightOuterRadius + rate);
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
        Destroy(this.gameObject, parSystem.main.startLifetime.constant);
    }
}