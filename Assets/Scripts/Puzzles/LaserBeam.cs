using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Beam header shot by the laser caster;
// Draws a colored trail that varies in width to simulate light;

public class LaserBeam : MonoBehaviour
{	
	// Speed of the beam. Inherited from the Caster;
	private float speed;

	// Duration of the beam trail. Inherited from the Caster;
	private float life;

	// Max and min width for the laser-like wiggle;
	[SerializeField] private float maxWidth;
	[SerializeField] private float minWidth;

	// Reference to the parent Terminal of the mirror system;
	private LaserTerminal parentTerminal;

	// Reference to the particle generator that will be spawned;
	[SerializeField] private GameObject laserParticles;

	// References to the rigid body, the trail renderer,
	// the object collider, and the particle system;
	private Rigidbody2D _rigidBody;
	private TrailRenderer _trailRenderer;
	private ParticleSystem _parSystem;
	private ParticleSystem.EmissionModule _emissions;

	// Stores how the beam has been active.
	// Passed onto the particles the beam generates.
	private float _timer;

	// Variables used to control the wiggle and behavior of the trail;
	private bool _widthSwipeUp = true;

	// Variable to store the outcome of the laser shot;
	private bool _succeeded = false;

	// Grab component references and set the life of the trail;
	void Awake() {
		_rigidBody = GetComponent<Rigidbody2D>();
		_trailRenderer = GetComponent<TrailRenderer>();
		_parSystem = GetComponent<ParticleSystem>();
		_emissions = _parSystem.emission;
	}

	// Initiates the motion of the beam;
	void Start() {
		_trailRenderer.widthMultiplier = 0;
		_rigidBody.AddForce(transform.right * speed);
		_trailRenderer.time = life*3f/4f;
	}

	// Controls the wiggle of the laser;
	void Update() {
		if (life > 0) {
			if ((_widthSwipeUp) && (_trailRenderer.widthMultiplier < maxWidth)) {
				_trailRenderer.widthMultiplier += Time.deltaTime;
			}
			else if ((_widthSwipeUp) && (_trailRenderer.widthMultiplier >= maxWidth)) {
				_widthSwipeUp = false;
				_trailRenderer.widthMultiplier -= Time.deltaTime;
			}
			else if ((!_widthSwipeUp) && (_trailRenderer.widthMultiplier > minWidth)) {
				_trailRenderer.widthMultiplier -= Time.deltaTime;
			}
			else if ((!_widthSwipeUp) && (_trailRenderer.widthMultiplier <= minWidth)) {
				_widthSwipeUp = true;
				_trailRenderer.widthMultiplier += Time.deltaTime;
			}
			_timer += Time.deltaTime;
			life -= Time.deltaTime;
		} // Destroys the laser a certain time after the projected life time runs out;
		if (life <= 0) {
			if (_timer > 0) {
				Destroy(gameObject, _trailRenderer.widthMultiplier);
				_timer = 0;
			}
			if (_trailRenderer.widthMultiplier > 0) {
				_trailRenderer.widthMultiplier -= Time.deltaTime;
			}
		}
	}

	// Creates a particle effect on collision and destroys on alien contact;
	void OnCollisionEnter2D(Collision2D collider) {
		if (collider.gameObject.tag == "Mirror") {
			Instantiate(laserParticles, transform.position, transform.rotation).GetComponent<LaserParticles>().life = life - _timer;
		}
		else if (collider.gameObject.tag == "Receiver") {
			collider.gameObject.GetComponent<Receiver>().feedback(0);
			_rigidBody.velocity = Vector3.zero;
			_succeeded = true;
			StartCoroutine(DisableHeader(_trailRenderer.time));
		}
		else {
			_rigidBody.velocity = Vector3.zero;
			StartCoroutine(DisableHeader(_trailRenderer.time));
		}
	}

	// Coroutine to stop the header particle system from emitting,
	// when the trails runs out;
	IEnumerator DisableHeader(float wait) {
		yield return new WaitForSeconds(wait);
		_emissions.enabled = false;
	}

	// Make the call to the parent terminal to react based on results;
	void OnDestroy() {
		if (_succeeded) {
			parentTerminal.PuzzleSuccess();
		}
		else {
			parentTerminal.PuzzleFailure();
		}
	}

	public void SetUpBeam(float speed, LaserTerminal parentTerminal, float life) {
		this.speed = speed;
		this.parentTerminal = parentTerminal;
		this.life = life;
	}
}
