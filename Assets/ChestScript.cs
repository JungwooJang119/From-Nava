using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChestScript : MonoBehaviour
{
    public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
	public GameObject _cameraTarget;				// The target the camera will move towards.
	public Transform _returnToPlayer;
    [SerializeField] private GameObject spellNotif;
    private bool isNear = false;
    public Animator animator;
    public GameObject door;

    // Button Tutorial Pop-Up
    [SerializeField] private GameObject buttonTutorial;
    private GameObject _tutInstance;
    private ButtonTutorial _tutScript;

    // Key to interact
    private string _intKey = "space";

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
        StartCoroutine(CameraTransitionIn());
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear) {
            if (!spellNotif.activeSelf) {
                if (_tutInstance == null) {
                    _tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
                    _tutScript = _tutInstance.GetComponent<ButtonTutorial>();
                    _tutScript.SetUp(_intKey, gameObject);
                }
                else {
                    _tutScript.CancelFade();
                }
            }
			if (Input.GetKeyDown(_intKey)) {
                StartCoroutine(TextPopUp());
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
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        _virtualCamera.Follow = _returnToPlayer;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            print("here");
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            isNear = false;
        }
    }

    IEnumerator TextPopUp() {
        spellNotif.SetActive(true);
        animator.SetBool("OpeningChest", true);
		AudioControl.Instance.PlaySFX("Chest Open");
		yield return new WaitForSeconds(4.0f);
        spellNotif.SetActive(false);
        animator.SetBool("OpeningChest", false);
        AudioControl.Instance.PlaySFX("Chest Close");
        door.GetComponent<Door>().OpenDoor();
    }
}
