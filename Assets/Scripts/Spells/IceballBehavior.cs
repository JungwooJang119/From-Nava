using UnityEngine;

public class IceballBehavior : BaseSpellBehavior {

	[Header("Iceball Behavior")]

	[SerializeField] private SpriteRenderer[] spriteRenderers;
	[SerializeField] private Transform innerTransform, outerTransform1, outerTransform2;
	[SerializeField] private float innerRotationSpeed = 25f;
	[SerializeField] private Vector2 outerRotationSpeed = new Vector2(-3.75f, 2.5f);

	protected override void Awake() {
		base.Awake();
		foreach (SpriteRenderer sr in spriteRenderers) {
			Color color = sr.color;
			color.a = 0;
			sr.color = color;
		} ShiftState(State.Start);
	}

	void Update() {
		RotateSprites(outerRotationSpeed.x, outerRotationSpeed.y, innerRotationSpeed);
		switch (state) {
			case State.Start:
				/// Stay on cast point until release;
				transform.position = castPoint.position;
				/// Deactivate spell on start;
				if (spellScript != null && spellScript.enabled) {
					ToggleComponents();
				} float avgOpacity = MoveOpacity(1f, 7.5f);
				if (Mathf.Approximately(avgOpacity, 1f)) {
					trailParticles.Play();
					/// Reactivate components;
					if (spellScript != null) ToggleComponents();
					LifetimeCountownAsync(spellData.lifetime);
					ShiftState(State.Lifetime);
				} break;
			case State.End:
				transform.localScale = Vector2.MoveTowards(transform.localScale, Vector2.zero, Time.deltaTime * 20f);
				if (Mathf.Approximately(Vector2.Distance(transform.localScale, Vector2.zero), 0)) {
					if (spellScript != null) {
						CleanUp();
						GenerateBurst(1f, 40f);
					} else {
						GenerateBurst(0.75f, 60f);
					} state = State.Done;
				} break;
		}
	}

    protected override void Spell_OnSpellDestroy(GameObject o) {
		AudioControl.Instance.PlaySFX("Iceball Collision", gameObject, 0.1f);
		base.Spell_OnSpellDestroy(o);
    }

	private float MoveOpacity(float target, float rate) {
		float avgAlpha = 0;
		foreach (SpriteRenderer sr in spriteRenderers) {
			Color color = sr.color;
			color.a = Mathf.MoveTowards(color.a, target, Time.deltaTime * rate);
			sr.color = color;
			avgAlpha += color.a;
		} return avgAlpha / spriteRenderers.Length;
	}

	private void RotateSprites(float rotationRateOuter1, float rotationRateOuter2, float rotationRateInner) {
		float deltaTime = Time.deltaTime * 60;
		innerTransform.Rotate(0, 0, rotationRateInner * deltaTime);
		outerTransform1.Rotate(0, 0, rotationRateOuter1 * deltaTime);
		outerTransform2.Rotate(0, 0, rotationRateOuter2 * deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.TryGetComponent(out BaseObject baseObject)) {
			baseObject.Freeze(this);
		}
	}
}