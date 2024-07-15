using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour
{
    private BaseObject[] objList;
    private float wait;
	private TransitionManager transition;
    void Awake() {
		objList = GetComponentsInChildren<BaseObject>();
	}

    void Start() {
		transition = ReferenceSingleton.Instance.transition;
	}

    public void StartReset() {
        enabled = true;
		wait = transition.FadeOut();
    }

    public void ResetScene() {
		enabled = true;
		wait = transition.FadeOut();
	}

    void Update() {
		if (wait > 0) {
			wait -= Time.deltaTime;
			if (wait <= 0) {
                foreach (BaseObject obj in objList) {
                    if (obj != null) {
				        obj.ObjectReset();
			        }
                }
                transition.FadeIn();
				enabled = false;
			}
		}
	}
}
