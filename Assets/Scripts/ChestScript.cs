using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChestScript : MonoBehaviour
{
    private ClaimCollectible collectible;
    private bool awaitingCollectible;

	private CinemachineVirtualCamera virtualCamera;
	private Transform returnToPlayer;
    private bool isNear = false;

    private Animator animator;
    [SerializeField] private GameObject door;

    // Button Tutorial Pop-Up
    [SerializeField] private GameObject buttonTutorial;
    private GameObject _tutInstance;
    private ButtonTutorial _tutScript;

    [SerializeField] private bool startsActive;

    // Key to interact
    private string intKey = "space";

    // Start is called before the first frame update
    void Start()
    {
        collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += ChestScript_OnCollectibleClaimed;

		virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        returnToPlayer = PlayerController.Instance.transform;
        animator = GetComponent<Animator>();
        if (!startsActive) StartCoroutine(CameraTransitionIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear) {
            if (!awaitingCollectible) {
                if (_tutInstance == null) {
                    _tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
                    _tutScript = _tutInstance.GetComponent<ButtonTutorial>();
                    _tutScript.SetUp(intKey, gameObject);
                } else {
                    _tutScript.CancelFade();
                }
			}
			if (Input.GetKeyDown(intKey) 
                && animator.GetCurrentAnimatorStateInfo(0).IsName("ChestIdle")) {
                StartCoroutine(AwaitCollectible());
				
                if (_tutInstance != null) {
					_tutScript.Fade();
				}
			}
		} else if (_tutInstance) {
			_tutScript.Fade();
		}
	}

    IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.0f);
		virtualCamera.Follow = transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
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
		collectible.Collect();
		awaitingCollectible = true;
		animator.SetBool("OpeningChest", true);
		AudioControl.Instance.PlaySFX("Chest Open", gameObject);
        while (awaitingCollectible) yield return null;
		animator.SetBool("OpeningChest", false);
		AudioControl.Instance.PlaySFX("Chest Close", gameObject);
		door.GetComponent<Door>().OpenDoor();
	}

    private void ChestScript_OnCollectibleClaimed() {
        awaitingCollectible = false;
    }
}
