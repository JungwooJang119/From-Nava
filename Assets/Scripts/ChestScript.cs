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
    [SerializeField] private GameObject[] doors;

    // Button Tutorial Pop-Up
    [SerializeField] private GameObject buttonTutorial;
    private GameObject _tutInstance;
    private ButtonTutorial _tutScript;

    [SerializeField] private bool startsActive;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool hasDoors;

    private Color spriteColor;
    private bool hasOpened;

    // Key to interact
    private string intKey = "space";

    // Start is called before the first frame update
    void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
        collectible = GetComponent<ClaimCollectible>();
        collectible.OnCollectibleClaimed += ChestScript_OnCollectibleClaimed;
        playerController = PlayerController.Instance;

		virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        returnToPlayer = PlayerController.Instance.transform;
        animator = GetComponent<Animator>();
        if (!startsActive) StartCoroutine(CameraTransitionIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear) {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ChestIdle")) {
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
        spriteColor.a = 0f;
        GetComponent<SpriteRenderer>().color = spriteColor;
        playerController.DeactivateMovement();
        while (spriteColor.a < 1.02f) {
            //print(spriteColor.a);
            spriteColor.a += 0.02f;
            GetComponent<SpriteRenderer>().color = spriteColor;
            yield return new WaitForSeconds(0.01f);
        }
		//yield return new WaitForSeconds(0.5f); //delay before moving camera //maybe dissolve/fade chest in over 0.5f seconds???
		virtualCamera.Follow = transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
        playerController.ActivateMovement();
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
		animator.SetBool("OpeningChest", true);
		AudioControl.Instance.PlaySFX("Chest Open", gameObject);
        var timer = 2.5f;
        while (awaitingCollectible || timer > 0) {
            if (timer > 0) timer -= Time.deltaTime;
            yield return null;
        } animator.SetBool("OpeningChest", false);
		AudioControl.Instance.PlaySFX("Chest Close", gameObject);
		if (hasDoors && !hasOpened) {
            foreach (GameObject door in doors) {
                door.GetComponent<Door>().OpenDoor();   
            }
            hasOpened = true;
        }
	}

    private void ChestScript_OnCollectibleClaimed() {
        awaitingCollectible = false;
    }
}