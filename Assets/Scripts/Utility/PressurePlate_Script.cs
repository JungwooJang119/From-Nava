using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_Script : MonoBehaviour
{
    [SerializeField] private Sprite unpressed, pressed;
    public bool isPressed = false;
    [SerializeField] private SpriteRenderer render;

    private List<GameObject> entitiesInsideCollider;

    [SerializeField] private float range = 2;
    [SerializeField] private GameObject textOverhead;
	private GameObject tutInstance;		// Reference to instantiate text pop-up;
	private TextOverhead tutScript;   // Reference to instantiated text script;
    [SerializeField] private string resetText;

    private Transform playerTransform;

    private void Start() {
        entitiesInsideCollider = new List<GameObject>();
        playerTransform = PlayerController.Instance.transform;
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
        if (textOverhead != null) {
		    if (((Vector2)playerTransform.position - (Vector2)transform.position).magnitude < range) {
			    if (tutInstance == null) {
				    tutInstance = Instantiate(textOverhead, transform.position, Quaternion.identity);
				    tutScript = tutInstance.GetComponent<TextOverhead>();
                    tutScript.SetUp(resetText, gameObject);
			    } else {
				    tutScript.CancelFade();
			    }
		    } else if (tutInstance) {
			    tutScript.Fade();
		    }
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