using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPatternSet : MonoBehaviour
{
    private ClaimCollectible collectible;
    private bool awaitingCollectible;

    private bool isNear = false;
    // Button Tutorial Pop-Up
    [SerializeField] private GameObject buttonTutorial;
    private GameObject _tutInstance;
    private ButtonTutorial _tutScript;

    // Key to interact
    private string intKey = "space";

    // Start is called before the first frame update
    void Start()
    {
        collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += ChestScript_OnCollectibleClaimed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear) {
            if (_tutInstance == null) {
                _tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity, transform);
                _tutScript = _tutInstance.GetComponent<ButtonTutorial>();
                _tutScript.SetUp(intKey, gameObject);
            } else {
                //_tutScript.CancelFade(); //CHECK WHY THIS IS PREVENTING FROM FADING, Possible solution, create a second animation of the exact same as current and have that be the check a la chest script?!
            }
			if (Input.GetKeyDown(intKey)) {
                StartCoroutine(AwaitCollectible());
                if (_tutInstance != null) {
					_tutScript.Fade();
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

    IEnumerator AwaitCollectible() {
		awaitingCollectible = true;
		collectible.Collect();
        var timer = 2.5f;
        while (awaitingCollectible || timer > 0) {
            if (timer > 0) timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
	}

    private void ChestScript_OnCollectibleClaimed() {
        awaitingCollectible = false;
    }
}
