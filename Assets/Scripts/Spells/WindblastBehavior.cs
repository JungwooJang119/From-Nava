using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modifies the default behavior of Windblast. Most spells follow the same pattern:
// Initialization, Lifetime, Death (just like your average dog), hence,
// some methods may be similar/identical to other spell's.
// Due to the fast nature of the Windblast, it does not have an initialization phase;

public class WindblastBehavior : MonoBehaviour {

	[SerializeField] private SpellScriptObj spellData;  // Windspell data from the SO;

    [SerializeField] public float pushDist = 0f;
    [SerializeField] public float pushSpd = 1f;

	// Simple state machine;
	private enum State {
		Lifetime,
		End,
		Done,
	}
	private State state;

	private Spell spell;                    // Reference to spell component in this gameObject;

	private float deathTimer;               // Timer to account for the spell lifetime;

	private float xScaleFactor;				// Variables to control scale;
	private float yScaleFactor;

	private ParticleSystem[] parSystems;    // Children Particle Systems. Reverb on 0, Trail on 1;

	// Have you heard of start, the method called before the first frame of Update? :O
	void Start() {
		// Suscribe to the OnDestroy event from Spell.cs
		spell = GetComponent<Spell>();
		spell.OnSpellDestroy += Spell_OnSpellDestroy;

		parSystems = GetComponentsInChildren<ParticleSystem>();
        // If the particles systems were fetched in the wrong order, swap them;
        if (!parSystems[0].main.startSize3D) {
            var par = parSystems[0];
            parSystems[0] = parSystems[1];
            parSystems[1] = par;
        }
        // Set the rotation of the reverb particles to the right rotation;
        var mainParReverb = parSystems[0].main;
        mainParReverb.startRotation = Mathf.Deg2Rad * (360 - transform.rotation.eulerAngles.z);

		// Variables to manage the scale;
		xScaleFactor = transform.localScale.x;
		yScaleFactor = transform.localScale.y;
	}

	// Wait until the spell's life runs out. If it didn't collide, reduce the x-scale of the sprite.
	// If it does collider, reduce the y-scale instead. Then, yeet all the unnecesary components and
	// set up a small particle burst for visual flavor. The spell shall die when all particles run out ;-;
	void Update() {
		switch (state) {

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
				if (yScaleFactor > 0) {
					ChangeYScale(-7.5f);
				} else {
					// Add noise to trail for the dissipation burst;
					var noiseParTrail = parSystems[1].noise;
					noiseParTrail.enabled = true;
					// Change the angle of the particle generator and stop spell;
					if (spell != null) {
						parSystems[1].gameObject.transform.Rotate(new Vector3(0, 0, 180f));
						CleanUp();
						GenerateBurst(0.75f, 1f);
					} else {
						GenerateBurst(0.75f, 1f);
					}
					state = State.Done;
				}
				break;

			case State.Done:
				// Stop emissions;
				parSystems[0].Stop();
				parSystems[1].Stop();
				break;
		}
	}

	// Method to take away functionality after the lifetime has been exhausted;
	private void CleanUp() {
		Destroy(spell);
		Destroy(GetComponent<Rigidbody2D>());
		Destroy(GetComponent<Collider2D>());
	}

	// Method called when the Spell script fires the destroy event! YEET!
	private void Spell_OnSpellDestroy(GameObject o) {
		AudioControl.Instance.PlaySFX("Windblast Collision", gameObject);
		deathTimer = spellData.lifetime;
		CleanUp();
	}

	// Method to change the y-scale of the sprite;
	private void ChangeYScale(float reductionMultiplier) {
		yScaleFactor = Mathf.Max(0, yScaleFactor + Time.deltaTime * reductionMultiplier);
		transform.localScale = new Vector2(xScaleFactor, yScaleFactor);
	}

	// Method to generate a particle burst;
	private void GenerateBurst(float particleLifetime, float particleSpeed) {
		var mainParTrail = parSystems[1].main;
		mainParTrail.startLifetime = particleLifetime;
		mainParTrail.startSpeed = particleSpeed;
		parSystems[1].emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 120) });
		parSystems[1].Stop();
		parSystems[1].Play();
		Destroy(this.gameObject, parSystems[1].main.startLifetime.constant);
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
		IPushable pushable = other.gameObject.GetComponent<IPushable>();
		if (pushable != null) {
			print(pushDist);
			pushable.Push(spell.direction, pushDist, pushSpd);
		}
		if (other.gameObject.GetComponent<Fan>() != null) {
			other.gameObject.GetComponent<Fan>().Blow();
		} 
    }
}
