using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Entry point for the Laser/Mirror system;
// Can be activated if the player is close enough,
// and presses a button (space by default);
// Can't be interacted with until a puzzle attempt is complete;
// Moves the camera towards a point in the scene for the
// duration of the puzzle attempt.
// NOTE: The camera position object must be placed manually on the Editor;

public class LaserTerminal : MonoBehaviour
{
	[SerializeField] private float range;				// How far can the player be from the terminal to trigger it;
	[SerializeField] private GameObject laserCaster;	// Laser Caster that with which this terminal communicates. Must be set on the inspector;
	[SerializeField] private bool canTrigger = true;	// Whether the terminal is interactable;
	[SerializeField] private Sprite sprComputerOn;
	[SerializeField] private Sprite sprComputerRight;
	[SerializeField] private Sprite sprComputerWrong;
	[SerializeField] private GameObject buttonTutorial;

	// Variables for camera transition
	[SerializeField] private string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
	[SerializeField] private GameObject _cameraTarget;				// The target the camera will move towards.
	private Transform _returnToPlayer;								// Stores the original follow that the camera shall return to.

	// Variables to react to the player in range;
	private Transform _player;
	private GameObject _tutInstance;
	private ButtonTutorial _tutScript;

	private SpriteRenderer _spriteRenderer;			// Sprite Renderer reference;
	private string _intKey = "space";				// Keycode of the key used for interactions;

	void Start() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
	}

	// Checks if the player is in range to interact, inspired by Grace's script;
	void Update() {
		if (((Vector2)_player.position - (Vector2)transform.position).magnitude < range && canTrigger) {
			if (_tutInstance == null) {
				_tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
				_tutScript = _tutInstance.GetComponent<ButtonTutorial>();
				_tutScript.keyToPress = _intKey;
				_tutScript.parent = gameObject;
			} else {
				_tutScript.CancelFade();
			}
			if (Input.GetKeyDown(_intKey)) {
				if (_tutInstance != null) {
					_tutScript.Fade();
				}
				_spriteRenderer.sprite = sprComputerOn;
				AudioControl.Instance.PlaySFX("Computer On");
				canTrigger = false;
				StartCoroutine(CameraTransitionIn());
			}
		} else if (_tutInstance) {
			_tutScript.Fade();
		}
	}

	// Corouting to start the camera transtion;
	IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.5f);
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(1.25f);
		laserCaster.GetComponent<LaserCaster>().LoadBeam();
	}

	// Coroutine to transition back if the puzzle is successful;
	IEnumerator CameraTransitionOutGood() {
		yield return new WaitForSeconds(0.5f);
		_spriteRenderer.sprite = sprComputerRight;
		AudioControl.Instance.PlaySFX("Computer Right");
		canTrigger = true;
	}

	// Coroutine to transition back if the puzzle fails;
	IEnumerator CameraTransitionOutBad() {
		yield return new WaitForSeconds(0.5f);
		_spriteRenderer.sprite = sprComputerWrong;
		AudioControl.Instance.PlaySFX("Computer Wrong");
		canTrigger = true;
	}

	// Called by the beam if the puzzle attempt succeeds;
	public void PuzzleSuccess() {
		_virtualCamera.Follow = _returnToPlayer;
		StartCoroutine(CameraTransitionOutGood());
	}

	// Called by the beam if the puzzle attempt fails;
	public void PuzzleFailure() {
		_virtualCamera.Follow = _returnToPlayer;
		StartCoroutine(CameraTransitionOutBad());
	}
}
