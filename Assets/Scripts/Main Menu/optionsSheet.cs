using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple script I wrote to move the Options sheet up and down. The sheet master approaches the position
// of two dummy target objects: OpTargetUp & OpTargetDown, which are set manually on the scene.
// The sheet changes state when the Options button and the Back button on the sheet are pressed;

public class optionsSheet : MonoBehaviour {
	int move = 0; // State variable. 1 == Moving Up | -1 == Moving Down | 0 == Not Moving;
	float speed = 1000; // Self-explanatory. Adjustable;
	public GameObject OpTargetUp; // Fetches a reference to a child
	public GameObject OpTargetDown; // dummy object whose position we want to reach;

	// Method to change the sheet's state to "moving up." Called by Options button;
	public void PullOptions() {
		move = 1;
	}

	// Method to change the sheet's state to moving down." Called by Back button;
	public void SinkOptions() {
		move = -1;
	}

	// Standard motion of sheet based on the state variable 'move';
	void Update() {
		if (move == 1) {
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, OpTargetUp.transform.position, step);
		} if (move == -1) {
			float step = speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, OpTargetDown.transform.position, step);
		}
	}

	// Stops the sheet from moving once the destination has been reached (collides with the corresponding target object);
	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log("yay");
		if ((move == 1 && coll.gameObject == OpTargetUp) || (move == -1 && coll.gameObject == OpTargetDown)) {
			move = 0;
		}
	}
}