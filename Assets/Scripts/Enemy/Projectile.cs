using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 4f;
    public int damage;

    private Vector3 playerPos;
    private Vector2 dir;

	// Simple state machine;
	private enum State {
		Lifetime,
		End,
		Done,
	} private State state;

    private float scaleFactor;
    private SpriteRenderer sprRenderer;
	private ParticleSystem parSystem;

	// Start is called before the first frame update
	void Start()
    {
        playerPos = PlayerController.Instance.transform.position;
        dir = (new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y)).normalized;

        sprRenderer = GetComponentInChildren<SpriteRenderer>();
		parSystem = GetComponentInChildren<ParticleSystem>();
        scaleFactor = transform.localScale.x;
	}

    // Update is called once per frame
    // Use Collider not position
    void Update()
    {
        sprRenderer.transform.Rotate(0, 0, 30f);
        switch (state) {
            case State.Lifetime:
                if (lifetime > 0) {
                    lifetime -= Time.deltaTime;
                    transform.Translate(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, transform.position.z);
                } else {
                    state = State.End;
                } break;
            case State.End:
                if (scaleFactor > 0) {
                    ChangeSize(0.1f);
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
        scaleFactor = Mathf.Max(0, scaleFactor - rate);
		transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
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
