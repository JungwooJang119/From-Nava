using UnityEngine;

public class FirewoodController : MonoBehaviour {

	private float wait;
	private Firewood[] firewoods;
	private TransitionManager transition;

	void Awake() {
		firewoods = GetComponentsInChildren<Firewood>();
	}

	void Start() {
		transition = ReferenceSingleton.Instance.transition;
	}

	public void PressPlate() {
		enabled = true;
		wait = transition.FadeOut();
	}

	void Update() {
		if (wait > 0) {
			wait -= Time.deltaTime;
			if (wait <= 0) {
				foreach (Firewood firewood in firewoods) {
					firewood.ObjectReset();
				} transition.FadeIn();
				enabled = false;
			}
		}
	}
}
