using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceballBehavior : MonoBehaviour {
    [SerializeField] private SpellScriptObj spellData;

    private Spell spellScript;          // Reference for Spell.cs;
    private float deathTimer;           // Timer to account for the spell lifetime;
    private float scaleFactor;          // Variable to control size decrease;
    private ParticleSystem parSystem;   // Reference to the particle system (child);
    private bool awaitForDeath;         // Lazy boolean, sad excuse for a state machine;
    private Transform[] transformArray; // Array of references to the transforms of children;

    // In today's news, start is called before the first frame update! O.O
    void Start() {
        // Suscribe to the OnDestroy event from Spell.cs
        spellScript = GetComponent<Spell>();
        spellScript.OnSpellDestroy += Spell_OnSpellDestroy;
        // Variables to manage the particle system and light source
        parSystem = GetComponentInChildren<ParticleSystem>();
        scaleFactor = transform.localScale.x;
        transformArray = new Transform[3];
        var i = 0;
        foreach (Transform t in transform) {
            transformArray[i] = t;
            i++;
        }
    }

    // Wait until the spell's life runs out. If it didn't collide, reduce the scale of the iceball sprites.
    // Then, yeet all the unnecesary components and set up a small particle burst for visual flavor.
    // The spell shall die when all particles run out ;-;
    void Update() {
        RotateSprites(-0.75f,0.5f,5f);
        if (deathTimer < spellData.lifetime) {
            deathTimer += Time.deltaTime;
        }
        else if (!awaitForDeath) {
			if (scaleFactor > 0) {
                ReduceSize(7.5f);
            }
            else {
				if (spellScript != null) {
                    CleanUp();
                    GenerateBurst(1f, 10f);
                } else {
					GenerateBurst(0.75f, 15f);
                }
                awaitForDeath = true;
            }
        }
        else {
            parSystem.Stop();
        }
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

    private void ReduceSize(float reductionMultiplier) {
        scaleFactor -= Time.deltaTime * reductionMultiplier;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    private void RotateSprites(float rotationRateOuter1, float rotationRateOuter2, float rotationRateInner) {
        int tranNumber = 0;
        foreach (Transform transform in transformArray) {
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