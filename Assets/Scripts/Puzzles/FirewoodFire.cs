using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirewoodFire : MonoBehaviour
{
	private enum State {
		StartGrowing,
		StartShrinking,
		Ending,
		Vibing,
		Idle,
	} private State state = State.Vibing;

	private float oscillation = 0.5f;
	private float targetScale;
	private float scaleRate = 6f;
	private float modifier = 1f;

	private ParticleSystem parSystem;
	private GameObject lightParent;
	private Light2D[] lightList;
	private float[] lightBounds;
	private float lightRate = 15f;

	// Variables to control the twist from the Windblast;
	private bool twisting = false;
	// 6 Variables Packed In Vector2's, see Start() for reference & changes;
	private Vector2 rotation;
	private Vector2 scale;
	private Vector2 rates;
	private float scaleVariance = 0.3f;
	// Note: Remind me to migrate this configuration to the inspector in the future;

	void Awake() {
		parSystem = transform.parent.GetComponentInChildren<ParticleSystem>(true);
		parSystem.Stop();
		targetScale = transform.localScale.x;

		lightParent = transform.parent.GetComponentInChildren<LightController>(true).gameObject;
		lightList = transform.parent.GetComponentsInChildren<Light2D>(true);
		lightBounds = new float[lightList.Length];
		for (int i = 0; i < lightList.Length; i++) {
			lightBounds[i] = lightList[i].pointLightOuterRadius;
		}

		// X -> Horizontal Rotation | Y -> Vertical Rotation;
		rotation = new Vector2(0, 35);
		// X -> X-Scale | Y -> Y-Scale;
		scale = new Vector2(1, 1);
		// X -> Rotation Rate |Y -> Scaling Rate;
		// Base values: (60, 0.45f);
		rates = new Vector2(300, 2.25f);
	}

	void OnDisable() { state = State.Idle; }

	// Entry point for the status change visual sequence;
	public void Toggle(bool active) {
		if (active) {
			if (state == State.Idle) {
				lightParent.SetActive(true);
				transform.localScale = Vector2.zero;
				for (int i = 0; i < lightList.Length; i++) {
					lightList[i].pointLightOuterRadius = 0;
				} modifier = 1f;
			} else {
				modifier = 1.25f;
			} state = State.StartGrowing;
		} else { state = State.Ending; }
	}

	// Declare the target rotation & scale for the twist;
	public void Twist(Vector3 direction) {
		// Would have looked nicer in a switch() but cardinal vectors are not constants ;-;
		if (direction == Vector3.right) {
			transform.eulerAngles = new Vector2(0, transform.eulerAngles.y);
			rotation = new Vector2(rotation.y, rotation.y);
			scale = new Vector2(1 + scaleVariance, 1 + scaleVariance);
		} else if (direction == Vector3.left) {
			rotation = new Vector2(-rotation.y, rotation.y);
			scale = new Vector2(1 + scaleVariance, 1 + scaleVariance * 2f);
		} else if (direction == Vector3.up) {
			rotation = new Vector2(0, rotation.y);
			scale = new Vector2(1 - scaleVariance / 2f, 1 + scaleVariance * 2f);
		} else if (direction == Vector3.down) {
			rotation = new Vector2(0, rotation.y);
			scale = new Vector2(1 + scaleVariance * 2f, 1 - scaleVariance * 2f);
		} else {
			Toggle(true);
			return;
		} twisting = true;
	}

	// What do Update() and the grind have in common? They never stop!
	void Update() {
		// Visuals for changing the firewood's lit status;
		var scaleRate = this.scaleRate * Time.deltaTime;
		switch (state) {
			case State.StartGrowing:
				ModifyLight(lightRate, true);
				transform.localScale = new Vector2(
					Approach(transform.localScale.x, targetScale + (oscillation / 3f) * modifier, scaleRate),
					Approach(transform.localScale.y, targetScale + oscillation * modifier, scaleRate)
				);
				if (transform.localScale.y == targetScale + oscillation * modifier) state = State.StartShrinking;
				break;
			case State.StartShrinking:
				ModifyLight(lightRate);
				transform.localScale = new Vector2(
					Approach(transform.localScale.x, targetScale, scaleRate / Mathf.Pow(modifier, 3)),
					Approach(transform.localScale.y, targetScale, scaleRate / Mathf.Pow(modifier, 3))
				);
				if (transform.localScale.y == targetScale) {
					state = State.Vibing;
				} break;
			case State.Ending:
				ModifyLight(-lightRate);
				transform.localScale = new Vector2(
					Approach(transform.localScale.x, 0, scaleRate * 1.5f),
					Approach(transform.localScale.y, 0, scaleRate * 1.5f)
				);
				if (transform.localScale.y == 0) {
					parSystem.Play();
					state = State.Idle;
					lightParent.SetActive(false);
					gameObject.SetActive(false);
				} break;
		}
		// Visuals for the Windblast interaction;
		if (twisting) {
			var rates = this.rates * Time.deltaTime;
			if (this.rates.x > 0) {
				transform.eulerAngles = new Vector3(Approach(transform.eulerAngles.x < 180 ? transform.eulerAngles.x : transform.eulerAngles.x - 360, 
															 rotation.x, rates.x),
													Approach(transform.eulerAngles.y, rotation.y, rates.x), 0);
				transform.localScale = new Vector3(Approach(transform.localScale.x, scale.x, rates.y),
												   Approach(transform.localScale.y, scale.y, rates.y), 0);
				if (transform.eulerAngles.y == rotation.y) this.rates.x = -this.rates.x;
			} else {
				transform.eulerAngles = new Vector3(Approach(transform.eulerAngles.x < 180 ? transform.eulerAngles.x : transform.eulerAngles.x - 360,
															 0, 1),
													Approach(transform.eulerAngles.y, 0, 1), 0);
				transform.localScale = new Vector3(Approach(transform.localScale.x, targetScale, rates.y),
												   Approach(transform.localScale.y, targetScale, rates.y), 0);
				if (transform.eulerAngles.y == 0) {
					this.rates.x = -this.rates.x;
					twisting = false;
				}
			}
		}
	}

	// Bring a value closer to another without overthrowing the limit;
	private float Approach(float currentValue, float targetValue, float rate) {
		rate = Mathf.Abs(rate);
		if (currentValue < targetValue) {
			currentValue += rate;
			if (currentValue > targetValue) return targetValue;
		} else {
			currentValue -= rate;
			if (currentValue < targetValue) return targetValue;
		} return currentValue;
	}

	// Modifies the outer radius of the light source;
	private void ModifyLight(float rate, bool oscillate = false) {
		rate *= Time.deltaTime;
		for (int i = 0; i < lightList.Length; i++) {
			if (rate > 0) {
				lightList[i].pointLightOuterRadius = Approach(lightList[i].pointLightOuterRadius, 
															  lightBounds[i] + (oscillate ? oscillation : 0) * modifier, Mathf.Abs(rate) / Mathf.Pow(modifier, 6));
			} else {
				lightList[i].pointLightOuterRadius = Approach(lightList[i].pointLightOuterRadius, 0, Mathf.Abs(rate) / Mathf.Pow(modifier, 8));
			}
		}
	}
}
