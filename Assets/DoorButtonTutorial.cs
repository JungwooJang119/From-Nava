using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonTutorial : MonoBehaviour
{
    private bool isNear;
    private GameObject _tutInstance;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject buttonTutorial;
    private ButtonTutorial _tutScript;
    [SerializeField] private GameObject trans;
    private float rotation;

    void Awake() {
        rotation = trans.transform.rotation.eulerAngles.z;
    }
    
    void Update() {
        if (isNear) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Door")) {
                if (_tutInstance == null) {
                    float xMod = 0;
                    float yMod = 0;
                    switch(rotation) {
                        case 0:
                            xMod = .8f;
                            break;
                        case 180:
                            xMod = -.8f;
                            break;
                        case 270:
                            yMod = -1.8f;
                            break;
                    }
                    _tutInstance = Instantiate(buttonTutorial, transform.position + new Vector3(xMod, yMod, 0), Quaternion.identity);
                    _tutScript = _tutInstance.GetComponent<ButtonTutorial>();
                    _tutScript.SetUp("space", gameObject);
                } else {
                    _tutScript.CancelFade();
                }
            }
            
        } else if (_tutInstance) {
			_tutScript.Fade();
		}
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            isNear = false;
        }
    }
}
