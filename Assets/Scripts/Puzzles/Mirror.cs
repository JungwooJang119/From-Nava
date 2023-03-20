using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mirror object off which laser bounce. Displaceable using Elizabeth's wind spell;
// Mimics most behaviors from 'Basic Enemy' and Elizabeth's script;

public class Mirror : MonoBehaviour
{
	public float pushMitigation; // Mitigates push;
	private bool isPushed;
	private float pushDist;
	private float pushSpd;
	private Vector3 pushDir;
	private Vector3 origin; // Stores the origin coordinate;

    void Awake() {
		origin = transform.position;
	    isPushed = false;
    }

	public void Push(Vector2 dir, float dist, float spd) {
		isPushed = true;
		pushDir = new Vector3(dir.x, dir.y, 0);
		pushDist = dist * pushMitigation;
		pushSpd = spd * pushMitigation;
	}

	public void PushTranslate() {
		if (pushDist <= 0) {
			isPushed = false;
		}
		else {
			transform.Translate(pushDir * pushSpd * Time.deltaTime, relativeTo:Space.World);
			pushDist -= (pushDir * pushSpd * Time.deltaTime).magnitude;
		}
	}

	// Resets the position of the object. Call by the Mirror Controller;
	public void reset() {
		transform.position = origin;
	}

	void Update() {
		if (isPushed) {
			PushTranslate();
		}
	}
}
