using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindblastBehavior : MonoBehaviour
{
	[SerializeField] private SpellScriptObj spellData;  // Windspell data from the SO;

    [SerializeField] public float pushDist = 0f;
    [SerializeField] public float pushSpd = 1f;

	private Spell spell;                    // Reference to spell component in this gameObject;
    private ParticleSystem[] parSystems;    // Children Particle Systems. Reverb on 0, Trail on 1;

	private float xScaleFactor;
	private float yScaleFactor;

	private float deathTimer;               // Timer to account for the spell lifetime;
	private bool awaitForDeath;             // Lazy boolean, sad excuse for a state machine;

	// Have you heard of start, the method called before the first frame of Update? :O
	void Start() {
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
		
		xScaleFactor = transform.localScale.x;
		yScaleFactor = transform.localScale.y;
	}

	// Wait until the spell's life runs out. If it didn't collide, reduce the x-scale of the sprite.
	// If it does collider, reduce the y-scale instead. Then, yeet all the unnecesary components and
	// set up a small particle burst for visual flavor. The spell shall die when all particles run out ;-;
	void Update() {
		if (deathTimer < spellData.lifetime) {
			deathTimer += Time.deltaTime;
		}
		else if (!awaitForDeath) {
			if (yScaleFactor > 0) {
				ReduceScale(7.5f);
			} else {
				var noiseParTrail = parSystems[1].noise;
				noiseParTrail.enabled = true;
				if (spell != null) {
					parSystems[1].gameObject.transform.Rotate(new Vector3(0,0,180f));
					CleanUp();
					GenerateBurst(0.75f, 1f);
				} else {
					GenerateBurst(0.75f, 1f);
				}
				awaitForDeath = true;
			}
		}
		else {
			parSystems[0].Stop();
			parSystems[1].Stop();
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
		deathTimer = spellData.lifetime;
		CleanUp();
	}

	private void ReduceScale(float reductionMultiplier) {
		yScaleFactor = Mathf.Max(0, yScaleFactor - Time.deltaTime * reductionMultiplier);
		transform.localScale = new Vector3(xScaleFactor, yScaleFactor, 1f);
	}

	private void GenerateBurst(float particleLifetime, float particleSpeed) {
		var mainParTrail = parSystems[1].main;
		mainParTrail.startLifetime = particleLifetime;
		mainParTrail.startSpeed = particleSpeed;
		parSystems[1].emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 60) });
		parSystems[1].Stop();
		parSystems[1].Play();
		Destroy(this.gameObject, parSystems[1].main.startLifetime.constant);
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().Push(spell.direction, pushDist, pushSpd);
        } else if (other.gameObject.CompareTag("Mirror")) {                                     //Mirror push adition, by Carlos.
            other.gameObject.GetComponent<Mirror>().Push(spell.direction, pushDist, pushSpd);   //Mirror script will mitigate push values.
        }
        if(other.gameObject.CompareTag("Fan")) {
            other.gameObject.GetComponent<Fan>().Blow();
        }
    }
}
