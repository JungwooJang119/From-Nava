using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Beam header shot by the laser caster;
// Draws a colored trail that varies in width to simulate light;

public class LaserBeam : MonoBehaviour
{
	// Max and min width for the laser-like wiggle;
	[SerializeField] private float trailWidth;
	[SerializeField] private float wiggleRadius;
	[SerializeField] private float endDelay = 3f;
	[SerializeField] private GameObject laserParticles;

	public event Action OnBeamEnd;

	// Variables inherited from the caster;
	private float speed;
	private float lifetime;
	private LaserTerminal parentTerminal;

	private Rigidbody2D rigidBody;
	private TrailRenderer trailRenderer;
	private ParticleSystem parSystem;

	// State machine!
	private enum Stage {
		Start,
		Cool,
		Wiggle,
		End,
	} private Stage stage;

	private bool hitReceiver = false;

	// Grab component references and set the lifetime of the trail;
	void Awake() {
		rigidBody = GetComponent<Rigidbody2D>();
		trailRenderer = GetComponent<TrailRenderer>();
		parSystem = GetComponent<ParticleSystem>();
	}

	// Initiates the motion of the beam;
	void Start() {
		trailRenderer.widthMultiplier = 0;
		rigidBody.AddForce(transform.right * speed);
		stage = Stage.Start;
	}

	// Controls THE ALMIGHTY LASER;
	void Update() {
		switch (stage) {
			case Stage.Start:
				// Increase width heavily after instantiation;
				if (trailRenderer.widthMultiplier < trailWidth * 2) {
					trailRenderer.widthMultiplier += Time.deltaTime;
				} else {
					stage = Stage.Cool;
				}
				break;
			case Stage.Cool:
				// Decrease size to match declared width;
				if (trailRenderer.widthMultiplier > trailWidth) {
					trailRenderer.widthMultiplier -= Time.deltaTime;
				} else {
					stage = Stage.Wiggle;
				}
				break;
			case Stage.Wiggle:
				// Calculate wiggle;
				trailRenderer.widthMultiplier = Mathf.Sin(Time.time * 7.5f) * wiggleRadius + trailWidth;
				// Destroy beam when lifetime runs out;
				if (lifetime < 0) {
					Destroy(gameObject, trailRenderer.widthMultiplier + 0.5f);
					OnBeamEnd?.Invoke();
					stage = Stage.End;
				}
				break;
			case Stage.End:
				// Decrease width to zero;
				if (trailRenderer.widthMultiplier > 0) {
					trailRenderer.widthMultiplier -= Time.deltaTime;
				}
				break;
		}
		if (lifetime > 0) lifetime -= Time.deltaTime;
	}

	// Creates a particle effect on collision and destroys on alien contact;
	void OnCollisionEnter2D(Collision2D collider) {
		if (collider.gameObject.tag == "Mirror") {
			var particle = Instantiate(laserParticles, transform.position, transform.rotation);
			particle.GetComponent<LaserParticles>().Subscribe2Parent(this);
		} else if (collider.gameObject.tag == "Receiver") {
			collider.gameObject.GetComponent<Receiver>().Feedback(0);
			rigidBody.velocity = Vector3.zero;
			hitReceiver = true;
			lifetime = endDelay;
			StartCoroutine(DisableHeader(endDelay));
		} else {
			rigidBody.velocity = Vector3.zero;
			lifetime = endDelay;
			StartCoroutine(DisableHeader(endDelay));
		}
	}

	IEnumerator DisableHeader(float wait) {
		yield return new WaitForSeconds(wait);
		var emission = parSystem.emission;
		emission.enabled = false;
	}

	// Make the call to the parent terminal to react based on results;
	void OnDestroy() {
		if (hitReceiver) {
			parentTerminal.PuzzleSuccess();
		} else {
			parentTerminal.PuzzleFailure();
		}
	}

	// Method to initialize a beam from LaserCaster.cs;
	public void SetUpBeam(float speed, LaserTerminal parentTerminal, float lifetime, Collider2D casterCollider) {
		this.speed = speed;
		this.parentTerminal = parentTerminal;
		this.lifetime = lifetime;
		Physics2D.IgnoreCollision(casterCollider, GetComponent<Collider2D>());
	}
}
