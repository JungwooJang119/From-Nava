using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent object of all the mirrors of a given puzzle;
// Restores the mirrors to their original position via the pressPlate() method;

public class MirrorController : MonoBehaviour
{
	// Amount of time to wait based on transition time;
	private float _wait;

	// Reference to all mirrors listed as children of the controller;
	private Mirror[] _allMirrors;

	// Reference to the transition prefab on the UI Canvas;
	private tranMode _transition;

	void Awake() {
		_allMirrors = GetComponentsInChildren<Mirror>();
	}
	
	void Start() {
		_transition = GameObject.Find("Transition").GetComponent<tranMode>();
	}
	
	// Method to call from the pressure plate;
	public void PressPlate() {
		_wait = _transition.FadeOut();
	}

	//Wait until screen is black to change the position of the mirrors;
	void Update() {
		if (_wait > 0) {
			_wait -= Time.deltaTime;
			if (_wait <= 0) {
				foreach (Mirror _mirror in _allMirrors) {
					_mirror.reset();
				}
				_transition.FadeIn();
			}
		}
	}
}
