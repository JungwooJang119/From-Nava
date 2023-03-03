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
	public float range;						// How far can the player be from the terminal to trigger it;
	public GameObject laserCaster;          // Laser Caster that with which this terminal communicates. Must be set on the inspector;
	public bool canTrigger = true;			// Whether the terminal is interactable;
	public SpriteRenderer spriteRenderer;	// Sprite rendering variables (to change the look of the terminal);
	public Sprite sprComputerOn;
	public Sprite sprComputerRight;
	public Sprite sprComputerWrong;

	// Variables for camera transition
	public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
	public GameObject _cameraTarget;				// The target the camera will move towards.
	private Transform _returnToPlayer;				// Stores the original follow that the camera shall return to.

	// Variables to calculate whether the player is in range, following Grace's script;
	private Transform _player;
	private float _currentDistance;

	void Start() {
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
	}

	// Utilizes Grace's Text Pop Script to check if the player is in range for interaction;
	void Update() {
		if ((Input.GetKeyDown(KeyCode.Space)) && canTrigger) {
			// Checks if player is near the object
			_currentDistance = ((Vector2)_player.position - (Vector2)transform.position).magnitude;
			// If the player is in range of the object
			if (_currentDistance < range) {
				spriteRenderer.sprite = sprComputerOn;
				AudioControl.Instance.PlaySFX("Computer On");
				canTrigger = false;
				StartCoroutine(CameraTransitionIn());
			}
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
		spriteRenderer.sprite = sprComputerRight;
		AudioControl.Instance.PlaySFX("Computer Right");
		canTrigger = true;
	}

	// Coroutine to transition back if the puzzle fails;
	IEnumerator CameraTransitionOutBad() {
		yield return new WaitForSeconds(0.5f);
		spriteRenderer.sprite = sprComputerWrong;
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
