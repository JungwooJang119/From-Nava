using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
	public int targetLevel;					// Index of the level to transition to;
	private GameObject _transitionRef;		// Reference to the Transition object;
	private SpriteRenderer _sprRenderer;	// Reference to the Sprite Renderer;

	// Initialize variables;
	void Start() {
		_sprRenderer = GetComponent<SpriteRenderer>();
		_sprRenderer.color = new Color32(0, 0, 0, 0);
	}

	// Triggers a transition on Collision;
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			_transitionRef = GameObject.Find("Transition");
			if (_transitionRef != null) {
				_transitionRef.GetComponent<tranMode>().LoadNext(targetLevel);
			} else {
				Debug.Log("Missing Transition Object");
			}
		}
	}
}