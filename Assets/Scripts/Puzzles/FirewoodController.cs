using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewoodController : MonoBehaviour {
	private float wait;
	private Firewood_Script[] firewoods;
	private TransitionManager transition;

	void Awake() {
		firewoods = GetComponentsInChildren<Firewood_Script>();
	}

	void Start() {
		transition = ReferenceSingleton.Instance.transition;
	}

	// Method to call from the pressure plate;
	public void PressPlate() {
		wait = transition.FadeOut();
	}

	//Wait until screen is black to change the position of the mirrors;
	void Update() {
		if (wait > 0) {
			wait -= Time.deltaTime;
			if (wait <= 0) {
				foreach (Firewood_Script firewood in firewoods) {
					firewood.SetDefaultLit();
				} transition.FadeIn();
			}
		}
	}
}
