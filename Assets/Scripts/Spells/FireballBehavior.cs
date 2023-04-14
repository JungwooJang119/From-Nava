using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour {
    [SerializeField] private SpellScriptObj spellData;

    private Spell spellScript;          // Reference for Spell.cs;
    private float deathTimer;           // Timer to account for the spell lifetime;
    private float scaleFactor;          // Variable to control size decrease;
    private ParticleSystem parSystem;   // Reference to the particle system (child);
    private bool awaitForDeath;         // Lazy boolean, sad excuse of a state machine;
    private UnityEngine.Rendering.Universal.Light2D[] lightList; // Array to store light references;

    // Did you know start is called before the first frame update? :V
    void Start() {
        // Suscribe to the OnDestroy event from Spell.cs
        spellScript = GetComponent<Spell>();
        spellScript.OnSpellDestroy += Spell_OnSpellDestroy;
        // Variables to manage the particle system and light source
        parSystem = GetComponentInChildren<ParticleSystem>();
        scaleFactor = transform.localScale.x;
        lightList = GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Wait until the spell's life runs out. If it didn't collide, reduce the scale of the fireball sprite
    // and reduce the radius of the light source to 0. Then, yeet all the unnecesary components and set up a
    // small particle burst for visual flavor. The spell shall die when all particles run out ;-;
    void Update() {
        if (deathTimer < spellData.lifetime) {
            deathTimer += Time.deltaTime;
        }
        else if (!awaitForDeath) {
			ModifyLight(-0.075f);
			if (scaleFactor > 0) {
                ReduceSize(10f);
            }
            else {
				if (spellScript != null) {
                    CleanUp();
                    GenerateBurst(1f, 100f);
                } else {
                    GenerateBurst(0.75f, 150f);
                }
                awaitForDeath = true;
            }
        }
        else {
			ModifyLight(-0.075f);
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

    private void ModifyLight(float rate) {
        foreach (UnityEngine.Rendering.Universal.Light2D light in lightList) {
            light.pointLightOuterRadius = Mathf.Max(0, light.pointLightOuterRadius + rate);
        }
    }

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