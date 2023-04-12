using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewoodController : MonoBehaviour
{
	// Amount of time to wait based on transition time;
	private float _wait;

	// Reference to all mirrors listed as children of the controller;
	private Firewood_Script[] _allFirewoods;

	// Reference to the transition prefab on the UI Canvas;
	private tranMode _transition;

	void Awake() {
		_allFirewoods = GetComponentsInChildren<Firewood_Script>();
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
				foreach (Firewood_Script _firewood in _allFirewoods) {
					// if (_firewood.isLit) {
					// 	_firewood.ChangeLit();
					// }
					_firewood.SetDefaultLit();
				}
				_transition.FadeIn();
			}
		}
	}
}
