using System.Collections;
using UnityEngine;

public class WindSwipeModule : ObjectModule {

	[SerializeField] private Transform swipeTransform;
	[SerializeField] private float baseRotation = 35f;
	[SerializeField] private Vector2 swipeRates = new (300, 2.25f);
	[SerializeField] private float scaleVariance = 0.3f;
	[SerializeField] private ParticleSystem windParticles;

	private enum State { Idle, Swipe, Normalize }
	private State state = State.Idle;
	private Vector2 baseScale;

    protected override void Awake() {
        base.Awake();
		baseScale = swipeTransform.localScale;
		if (baseObject.ParentObject) {
			baseObject.ParentObject.OnBlow += BaseObject_OnBlow;
		} else baseObject.OnBlow += BaseObject_OnBlow;
	}

    private void BaseObject_OnBlow(Vector2 direction, float strength) {
		if (!gameObject.activeSelf || baseObject.State == ObjectState.Frozen) return;
		Vector2 swipeRotation = direction == Vector2.right ? new Vector2(baseRotation, baseRotation)
							  : direction == Vector2.left  ? new Vector2(-baseRotation + 360, baseRotation)
							  : direction == Vector2.down  ? new Vector2(0, baseRotation / 2f)
														   : new Vector2(0, baseRotation);
		Vector2 swipeScale = direction == Vector2.right ? new Vector2(1 + scaleVariance, 1 + scaleVariance)
						   : direction == Vector2.left ? new Vector2(1 + scaleVariance, 1 + scaleVariance)
						   : direction == Vector2.up ? new Vector2(1 - scaleVariance / 2f, 1 + scaleVariance)
						   : direction == Vector2.down ? new Vector2(1 + scaleVariance / 1.5f, 1 - scaleVariance)
						   : baseScale;
		state = State.Swipe;
		StopAllCoroutines();
		StartCoroutine(WindSwipe(Quaternion.Euler(swipeRotation), swipeScale));
	}

    private IEnumerator WindSwipe(Quaternion rotation, Vector2 scale) {
		while (state != State.Idle) {
			/// Rate Vector : x -> rotationRate , y -> scaleRate;
			Vector2 rates = swipeRates * Time.deltaTime;
			Quaternion targetRotation = state == State.Swipe ? rotation : Quaternion.identity;
			Vector2 targetScale = state == State.Swipe ? scale : baseScale;
			swipeTransform.rotation = Quaternion.RotateTowards(swipeTransform.rotation, targetRotation, rates.x);
			swipeTransform.localScale = Vector2.MoveTowards(swipeTransform.localScale, targetScale, rates.y);
			if (Quaternion.Angle(swipeTransform.rotation, targetRotation) == 0
				&& Vector2.Distance(swipeTransform.localScale, targetScale) == 0) {
				state = state switch { State.Swipe => State.Normalize, State.Normalize => State.Idle, _ => State.Idle };
			} yield return null;
		}
	}
}