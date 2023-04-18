using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Laser Caster: Loads and casts the beam on terminal (game object) input;

public class LaserCaster : MonoBehaviour
{
	// Speed of the laser launched defined on the inspector;
	[SerializeField] private float speed;
	// Projected life of the beam, passed onto the beam when the object is created;
	[SerializeField] private float beamLife;
	// Aesthetic wait after the SFX fades away;
	[SerializeField] private float launchDelay;

	// A bunch of required object references;
	[SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject laserWave;

	// Reference to associated objects;
	private LaserTerminal laserTerminal;
	private ParticleSystem parSystem;
	private Transform laserCastPoint;
	private GameObject currentLaser;

	// Simple state machine to control the loading sequence of the laser;
	private enum Stage {
		Idle,
		Start,
		Launch,
		Done,
	} private Stage stage;
	private float timer = 0;

	void Start() {
		stage = Stage.Idle;
		laserTerminal = transform.parent.GetComponent<LaserTerminal>();
		// Fetching references from children. Funky way to do it, but looks tidy doesn't it? :D
		for (int i = 0; i < transform.childCount; i++) {
			var componentCheck = transform.GetChild(i).GetComponent<ParticleSystem>();
			if (componentCheck != null) {
				parSystem = componentCheck;
			} else {
				laserCastPoint = transform.GetChild(i);
			}
		}
		// Stop emissions;
		var emission = parSystem.emission;
		emission.enabled = false;
	}
	
	// Load the shot and cast the beam [aesthetically];
	void Update() {
		switch (stage) {
			case Stage.Start:
				if (timer > 0) {
					timer -= Time.deltaTime;
				} else {
					// Stop emissions;
					var emission = parSystem.emission;
					emission.enabled = false;
					// Set up wait timer;
					timer = launchDelay;
					stage = Stage.Launch;
				}
				break;
			case Stage.Launch:
				if (timer > 0) {
					timer -= Time.deltaTime;
				}
				else {
					// The beam gets instantiated, all done;
					InitiateBeam();
					stage = Stage.Done;
				}
				break;
		}
	}

	// Method called from the terminal, initiates the shot by loading the caster first;
	public void LoadBeam() {
		timer = AudioControl.Instance.PlaySFX("Loading Shot", laserTerminal.gameObject);
		// Start particle emissions;
		var emission = parSystem.emission;
		emission.enabled = true;
		// Spawn a wave on the caster [aesthetically];
		Instantiate(laserWave, transform.position, transform.rotation);
		stage = Stage.Start;
	}

	// Method to cast the beam;
	private void InitiateBeam() {
		AudioControl.Instance.PlaySFX("Final Shot", laserTerminal.gameObject);
		currentLaser = Instantiate(laserBeam, laserCastPoint.position, transform.rotation);
		// Pass required values to the laser beam. For more information, visit LaserBeam.cs;
		currentLaser.GetComponent<LaserBeam>().SetUpBeam(speed, laserTerminal, beamLife * 1.01f, GetComponent<Collider2D>());
	}
}