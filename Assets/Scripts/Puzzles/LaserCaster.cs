using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Laser Caster: Loads and casts the beam on terminal (game object) input;

public class LaserCaster : MonoBehaviour
{
	// Speed of the laser launched defined on the inspector;
	public float speed;

    // A bunch of required object references;
    public GameObject laserBeam;
    public GameObject laserWave;
    public GameObject laserParticles;
	public GameObject laserTerminal;

	// Reference to the script of the beam casted;
    private LaserBeam _beamScript;

	// Projected life of the beam,
	// passed onto the beam when the object is created;
	private float _beamLife;

	// Reference to the beam casted and the parent terminal;
	private GameObject _currentLaser;
    private GameObject _currentTerminal;

	// Variables to control the looping of loading sounds;
	private bool _load = false;
    private float _fxDuration = 0;
    private int _fxAllocations;
    private int _fxAllocationsMax = 4;

	// Method to cast the beam;
	public void InitiateBeam() {
		_beamLife = AudioControl.Instance.PlaySFX("Final Shot");
		_currentLaser = Instantiate(laserBeam, transform.position, transform.rotation);
		_beamScript = _currentLaser.GetComponent<LaserBeam>();
		_beamScript.speed = speed;
		_beamScript.parentTerminal = laserTerminal.GetComponent<LaserTerminal>();
		_beamScript.life = _beamLife * 1.01f;
	}

	// Method called from the terminal, initiates the shot by loading the caster first;
	// Note: This sequence used to be required to wait for the dummy laser to find a
	//		 target. It was later used to add some delay for the beam to find its way
	//		 before tracing. Now, it is merely aesthetic;
	public void LoadBeam() {
		_fxAllocations = _fxAllocationsMax;
		_load = true;
	}
	
	// Load the shot and cast the beam [aesthetic load];
	void Update() {
        if ((_fxDuration > 0) && (_load)) {
            _fxDuration -= Time.deltaTime;
        } else if ((_fxDuration <= 0) && (_load) && (_fxAllocations > 0)) {
			_fxDuration = 8f/9f * AudioControl.Instance.PlaySFX("Loading Shot");
			Instantiate(laserWave, transform.position, transform.rotation);
            _fxAllocations--;
		} else if ((_load) && (_fxAllocations <= 0)) {
			_load = false;
			InitiateBeam();
		}
	}
}