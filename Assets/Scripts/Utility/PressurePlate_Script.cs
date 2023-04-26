using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_Script : MonoBehaviour
{
    [SerializeField] private Sprite unpressed, pressed;
    public bool isPressed = false;
    [SerializeField] private SpriteRenderer render;

    private List<GameObject> entitiesInsideCollider;

    private void Start() {
        entitiesInsideCollider = new List<GameObject>();
    }

    void Update() {
        if (entitiesInsideCollider.Count > 0) {
            for (int i = 0; i < entitiesInsideCollider.Count; i++) {
                if (!entitiesInsideCollider[i]) entitiesInsideCollider.RemoveAt(i);
            }
        } else {
			isPressed = false;
			render.sprite = unpressed;
		}
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell") {
            return;
        }
        if (other.tag == "Melee") {
            return;
        }
        if (other.attachedRigidbody.mass > 1){
            entitiesInsideCollider.Add(other.gameObject);
            render.sprite = pressed;
            isPressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
		if (entitiesInsideCollider.Contains(other.gameObject)) {
            entitiesInsideCollider.Remove(other.gameObject);
        }
    }

    public bool GetIsPressed() {
        return isPressed;
    }
}