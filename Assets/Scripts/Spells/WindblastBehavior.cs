using System.Linq;
using UnityEngine;

public class WindblastBehavior : BaseSpellBehavior {

	[Header("Windblast Behavior")]

	[SerializeField] private ParticleSystem centerTrail;
    [SerializeField] private float pushStrength = 5f;

	protected override void Awake() {
		base.Awake();
		LifetimeCountownAsync(spellData.lifetime);
	}

    void Start() {
		var mainParReverb = centerTrail.main;
		mainParReverb.startRotation = Mathf.Deg2Rad * (360 - transform.rotation.eulerAngles.z);
	}

    void Update() {
		if (state == State.End) {
			transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(transform.localScale.x, 0), Time.deltaTime * 7.5f);
			if (Mathf.Approximately(transform.localScale.y, 0)) {
				/// Add noise to trail for the dissipation burst;
				var noiseParTrail = trailParticles.noise;
				noiseParTrail.enabled = true;
				/// Change the angle of the particle generator and stop spell;
				if (spellScript != null) {
					trailParticles.transform.Rotate(new Vector3(0, 0, 180f));
					CleanUp();
				} GenerateBurst(0.75f, 1f);
				ShiftState(State.Done);
			}
		}
	}

	protected override void Spell_OnSpellDestroy(GameObject o) {
		AudioControl.Instance.PlaySFX("Windblast Collision", gameObject);
		base.Spell_OnSpellDestroy(o);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (spellScript.CasterColliders != null 
			&& spellScript.CasterColliders.Contains(collision)) return;
		if (collision.TryGetComponent(out BaseObject baseObject)) {
			baseObject.Blow(this, spellScript.direction, pushStrength);
			if (baseObject.Attributes.isHeavy) state = State.End;
		} else if (collision.TryGetComponent(out IPushable pushable)) {
			Debug.Log($"{this}\n{spellScript.CasterColliders}");
			pushable.Push(spellScript.direction, pushStrength, pushStrength);
			state = State.End;
		}
    }
}
