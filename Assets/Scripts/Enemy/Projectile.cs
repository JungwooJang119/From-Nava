using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 4f;
    public int damage;

    private Vector3 playerPos;
    private Vector2 dir;

	// Simple state machine;
	private enum State {
        Start,
		Lifetime,
		End,
		Done,
	} private State state;

    private float scaleFactor;
    private float maxFactor;
    private SpriteRenderer sprRenderer;
	private ParticleSystem parSystem;

    private bool hasLight;
    private Light2D[] lightList;
	private float[] lightBounds;        // Target outer radii of light sources;

	// Start is called before the first frame update
	void Start()
    {
        playerPos = PlayerController.Instance.transform.position;
        dir = (new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y)).normalized;

        sprRenderer = GetComponentInChildren<SpriteRenderer>();
		parSystem = GetComponentInChildren<ParticleSystem>();
		var ps = parSystem.emission;
		ps.enabled = false;

		maxFactor = transform.localScale.x;
        scaleFactor = 0;

        hasLight = GetComponentInChildren<Light2D>() != null;

		if (hasLight) {
			lightList = GetComponentsInChildren<UnityEngine.Rendering.Universal.Light2D>();
			lightBounds = new float[lightList.Length];
			for (int i = 0; i < lightList.Length; i++) {
				lightBounds[i] = lightList[i].pointLightOuterRadius;
				lightList[i].pointLightOuterRadius = 0;
			}
		}
	}

    // Update is called once per frame
    // Use Collider not position
    void Update()
    {
        sprRenderer.transform.Rotate(0, 0, 30f);
        switch (state) {
            case State.Start:
                if (hasLight) ModifyLight(0.375f);
				if (scaleFactor < maxFactor) {
					ChangeSize(0.2f);
				} else {
					var ps = parSystem.emission;
					ps.enabled = true;
					state = State.Lifetime;
				} break;
			case State.Lifetime:
                if (lifetime > 0) {
                    lifetime -= Time.deltaTime;
                    transform.Translate(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, transform.position.z);
                } else {
                    state = State.End;
                } break;
            case State.End:
				if (hasLight) ModifyLight(-0.375f);
				if (scaleFactor > 0) {
                    ChangeSize(-0.2f);
                } else {
                    CleanUp();
                    if (lifetime > 0) {
                        GenerateBurst(0.5f, 40f);
                    } else {
                        GenerateBurst(0.75f, 20f);
                    } state = State.Done;
                } break;
            case State.Done:
                parSystem.Stop();
                break;
        }
    }
    
    //destroy the gameobject on collision with the player, make them take appropriate damage
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            return;
        }
        if (col.gameObject.CompareTag("EnemyProjectile")) {
            return;
        }
        if (col.gameObject.CompareTag("IceTower")) {
            return;
        }
        if (col.gameObject.CompareTag("Chest")) {
            return;
        }
        if (col.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
        state = State.End;
    }

    private void CleanUp() {
		Destroy(GetComponent<Rigidbody2D>());
		Destroy(GetComponent<Collider2D>());
	}

	private void ChangeSize(float rate) {
        if (rate > 0) {
			scaleFactor = Mathf.Min(maxFactor, scaleFactor + rate);
        } else {
            scaleFactor = Mathf.Max(0, scaleFactor + rate);
		}
        
		transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
	}

	// Method to modify the light sources attached to the spell;
	private void ModifyLight(float rate) {
		for (int i = 0; i < lightList.Length; i++) {
			if (lightBounds[0] > 0) {
				lightList[i].pointLightOuterRadius = Mathf.Min(lightBounds[i], lightList[i].pointLightOuterRadius + rate);
			}
			else {
				lightList[i].pointLightOuterRadius = Mathf.Max(lightBounds[i], lightList[i].pointLightOuterRadius + rate);
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
