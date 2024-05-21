using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirewoodFire : BaseObject {

	[SerializeField] private float baseOscillation = 0.5f;
	[SerializeField] private float scaleRate = 6f;
	[SerializeField] private float lightRate = 15f;

	[SerializeField] private ParticleSystem smokeParticles;
	[SerializeField] private LightController lightController;

	private float modifier = 1f;
	private Vector2 baseScale;
	private Vector2 prevScale;

	private Light2D[] lightList;
	private float[] lightBounds;

	private enum FireState {
		Idle,
		Grow,
		Normalize,
		Vibe,
		End,
	} private FireState state = FireState.Vibe;

	void Awake() {
		baseScale = transform.localScale;
		lightList = lightController.GetComponentsInChildren<Light2D>(true);
		lightBounds = new float[lightList.Length];
		for (int i = 0; i < lightList.Length; i++) {
			lightBounds[i] = lightList[i].pointLightOuterRadius;
		}
	}

	void Update() {
		// Visuals for changing the firewood's lit status;
		if (state != FireState.Vibe) {
			Vector2 targetScale = state switch {
				FireState.Grow => new(baseScale.x + (baseOscillation / 3f) * modifier, baseScale.y + baseOscillation * modifier),
				FireState.Normalize => baseScale,
				FireState.End => Vector2.zero,
				_ => transform.localScale
			}; transform.localScale = Vector2.MoveTowards(transform.localScale, targetScale, Time.deltaTime * scaleRate);
			/// Light Intensity scales based off how far the scaling is from completion, for consistency;
			float lerpValue = Vector2.Distance(transform.localScale, targetScale) / Vector2.Distance(prevScale, targetScale);
			LerpLightIntensity(lerpValue, baseOscillation);
			if (Mathf.Approximately(lerpValue, 1)) {
				FireState nextState = state switch {
					FireState.Grow => FireState.End,
					FireState.Normalize => FireState.Vibe,
					FireState.End => FireState.Idle,
					_ => state
				}; ShiftState(nextState);
			}
		}
	}

	private void ShiftState(FireState state) {
		prevScale = transform.localScale;
		this.state = state;
    }

	private void LerpLightIntensity(float lerpValue, float oscillation = 0) {
		for (int i = 0; i < lightList.Length; i++) {
			lightList[i].pointLightOuterRadius = Mathf.Lerp(0, lightBounds[i] + oscillation, lerpValue);
		}
	}
}

public class WindSwipeModule : ObjectModule {

	[SerializeField] private Transform swipeTransform;
	[SerializeField] private float baseRotation = 35f;
	[SerializeField] private Vector2 swipeRates = new (300, 2.25f);
	[SerializeField] private float scaleVariance = 0.3f;

	private enum State { Idle, Swipe, Normalize }
	private State state = State.Idle;
	private Vector2 baseScale;

	private void Start() {
		baseScale = swipeTransform.localScale;
		baseObject.OnBlow += BaseObject_OnBlow;
	}

	private void BaseObject_OnBlow(Vector2 direction, float strength) {
		if (baseObject.State == ObjectState.Frozen) return;
		Vector2 swipeRotation = direction == Vector2.right ? new Vector2(baseRotation, baseRotation)
							  : direction == Vector2.left  ? new Vector2(-baseRotation, baseRotation)
														   : new Vector2(0, baseRotation);
		Vector2 swipeScale = direction == Vector2.right ? new Vector2(1 + scaleVariance, 1 + scaleVariance)
						   : direction == Vector2.left ? new Vector2(1 + scaleVariance, 1 + scaleVariance * 2f)
						   : direction == Vector2.up ? new Vector2(1 - scaleVariance / 2f, 1 + scaleVariance * 2f)
						   : direction == Vector2.down ? new Vector2(1 + scaleVariance * 2f, 1 - scaleVariance * 2f)
						   : baseScale;
		state = State.Swipe;
		StopAllCoroutines();
		StartCoroutine(WindSwipe(swipeRotation, swipeScale));
	}

    private IEnumerator WindSwipe(Vector2 rotation, Vector2 scale) {
		while (state != State.Idle) {
			/// Rate Vector : x -> rotationRate , y -> scaleRate;
			Vector2 rates = swipeRates * Time.deltaTime;
			float xEuler = swipeTransform.localEulerAngles.x < 180 ? swipeTransform.localEulerAngles.x : swipeTransform.localEulerAngles.x - 360;

			Vector2 targetRotation = state == State.Swipe ? rotation : Vector2.zero;
			Vector2 targetScale = state == State.Swipe ? scale : baseScale;
			swipeTransform.localEulerAngles = new Vector2(Mathf.MoveTowards(xEuler, targetRotation.x, rates.x),
														Mathf.MoveTowards(swipeTransform.localEulerAngles.y, targetRotation.y, rates.x));
			swipeTransform.localScale = Vector2.MoveTowards(swipeTransform.localScale, targetScale, rates.y);
			if (Mathf.Approximately(Vector2.Distance(swipeTransform.localEulerAngles, targetRotation), 0)) {
				state = state switch { State.Swipe => State.Normalize, State.Normalize => State.Idle, _ => State.Idle };
			} yield return null;
		}
	}
}