using UnityEngine;

public class FireFlame : BaseObject {

	/// <summary> Invoked on disable for animation purposes; 
	/// <br></br> Received by the Flammable Module; </summary>
	public event System.Action OnFlameEnd;

	[SerializeField] private float baseOscillation = 0.6f;
	[SerializeField] private float scaleRate = 6f;

	[SerializeField] private ParticleSystem smokeParticles;
	[SerializeField] private LightController lightController;

	private float modifier = 1f;
	private Vector2 baseScale;

	private enum FireState {
		Idle,
		Grow,
		Normalize,
		Vibe,
		End,
	} private FireState state = FireState.Idle;

	protected override void Awake() {
		base.Awake();
		baseScale = transform.localScale;
	}

    protected override void Start() {
		if (!ParentObject) OnHeatToggle += Toggle;
		base.Start();
	}

    void OnDisable() { state = FireState.Idle; }

    void Update() {
		if (state == FireState.Idle) return;
		if (state != FireState.Vibe) {
			Vector2 targetScale = state switch {
				FireState.Grow => new(baseScale.x + (baseOscillation / 3f) * modifier, baseScale.y + baseOscillation * modifier),
				FireState.Normalize => baseScale,
				FireState.End => Vector2.zero,
				_ => transform.localScale
			}; float scaleRate = this.scaleRate * state switch {
				FireState.Normalize => 0.6f,
				FireState.End => 1.2f,
				_ => 1f,
            }; transform.localScale = Vector2.MoveTowards(transform.localScale, targetScale, Time.deltaTime * scaleRate);
			/// Light Intensity scales based off how far the scaling is from completion, for consistency;
			if (Mathf.Approximately(Vector2.Distance(transform.localScale, targetScale), 0)) {
				FireState nextState = state switch {
					FireState.Grow => FireState.Normalize,
					FireState.Normalize => FireState.Vibe,
					FireState.End => FireState.Idle,
					_ => state
				}; ShiftState(nextState);
			}
		}
	}

    public override void Ignite(MonoBehaviour trigger) {
		if (ParentObject) ParentObject.Ignite(trigger);
		else base.Ignite(trigger);
    }

    public override void Freeze(MonoBehaviour trigger) {
		if (ParentObject) ParentObject.Freeze(trigger);
		else base.Freeze(trigger);
	}

	/// <summary>
	/// Signal to grow or end the flame;
	/// </summary>
	/// <param name="active"> True: Grows the flame;
	///	<br></br> False: Kills the flame; </param>
	public void Toggle(ObjectState prevState, bool active) {
		if (active) {
			if (state == FireState.Idle) {
				gameObject.SetActive(true);
				lightController.gameObject.SetActive(true);
				transform.localScale = Vector2.zero;
				lightController.SetLightBounds(0, 0);
				modifier = 1f;
				if (ParentObject) ParentObject.SetState(ObjectState.Burning);
			} else { modifier = 1.25f; }
			ShiftState(FireState.Grow);
		} else {
			if (ParentObject) ParentObject.SetState(ObjectState.Default);
			ShiftState(FireState.End);
		}
	}

	/// <summary>
	/// Changes the current state and records values for interpolation;
	/// </summary>
	/// <param name="state"> New state; </param>
	private void ShiftState(FireState state) {
		if (this.state == state) return;
		if (state != FireState.Vibe) {
			lightController.MoveLightBounds(this.state == FireState.Vibe && state == FireState.Grow ? 0.35f
											: this.state == FireState.Grow && state == FireState.Normalize ? 0.2f : 1f,
											state == FireState.End ? 0 : 1,
											state == FireState.Grow ? baseOscillation * 2f * modifier : 0);
		} this.state = state;
		if (state == FireState.Idle) {
			OnFlameEnd?.Invoke();
			smokeParticles.Play();
			gameObject.SetActive(false);
			lightController.gameObject.SetActive(false);
		}
	}
}