using System.Collections;
using System.Collections.Generic;
using static RoomLights;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

// Entry point for the Laser/Mirror system;
// Can be activated if the player is close enough,
// and presses a button (space by default);
// Can't be interacted with until a puzzle attempt is complete;
// Moves the camera towards a point in the scene for the
// duration of the puzzle attempt.
// NOTE: The camera position object must be placed manually on the Editor;

public class LaserTerminal : IInteractable
{
	[SerializeField] private RoomCode revealRoomCode;
	private LaserCaster laserCaster;					// Laser Caster that with which this terminal communicates. Must be set on the inspector;
	[SerializeField] private Sprite sprComputerOn;
	[SerializeField] private Sprite sprComputerRight;
    [SerializeField] private Sprite sprComputerWrong;
	[SerializeField] private float maxZoom;
	[SerializeField] private float zoomSpeed;

	// Variables for camera transition
	private CinemachineVirtualCamera _virtualCamera;
	private CameraZoomController cameraZoom;
	private GameObject _cameraTarget;								// The target the camera will move towards.

	// Variables to react to the player in range;
	private Transform _player;
	private PlayerController _playerController;

	private SpriteRenderer _spriteRenderer;			// Sprite Renderer reference;

	public GameObject _cameraTarget2;				// The target the camera will move towards.
    public GameObject door;

	private bool roomComplete;
	public event Action OnFinish;

	void Start() {
		laserCaster = GetComponentInChildren<LaserCaster>();
		_cameraTarget = transform.Find("CameraTarget").gameObject;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_player = PlayerController.Instance.transform;
		_playerController = PlayerController.Instance;
		_virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
		cameraZoom = ReferenceSingleton.Instance.mainCamera.GetComponent<CameraZoomController>();
	}

    protected override void InteractBehavior() {
        _playerController.DeactivateMovement();
        FadeButton();
        if (!roomComplete) {
            _spriteRenderer.sprite = sprComputerOn;
            AudioControl.Instance.PlaySFX("Computer On", gameObject, 0f, 0.8f);
            canTrigger = false;
            StartCoroutine(CameraTransitionIn());
        }
    }

    // Corouting to start the camera transtion;
    IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.5f);
		cameraZoom.BeginZoom(maxZoom, zoomSpeed);
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(1.25f);
		laserCaster.GetComponent<LaserCaster>().LoadBeam();
	}

	// Coroutine to transition back if the puzzle is successful;
	IEnumerator CameraTransitionOutGood() {
		cameraZoom.RestoreZoom(zoomSpeed);
		yield return new WaitForSeconds(0.5f);
		_spriteRenderer.sprite = sprComputerRight;
		AudioControl.Instance.PlaySFX("Computer Right", gameObject);
		yield return new WaitForSeconds(1f);
		_virtualCamera.Follow = _cameraTarget2.transform;
		AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
		if (door != null) {
			door.GetComponent<Door>().OpenDoor();
			ReferenceSingleton.Instance.roomLights.Propagate(revealRoomCode);
		}
		if (_cameraTarget2 != null) {
			_cameraTarget2.SetActive(true);
		}
		yield return new WaitForSeconds(2f);
        _virtualCamera.Follow = _player;
		canTrigger = false;
		roomComplete = true;
	}

	// Coroutine to transition back if the puzzle fails;
	IEnumerator CameraTransitionOutBad() {
		yield return new WaitForSeconds(0.5f);
		_spriteRenderer.sprite = sprComputerWrong;
		AudioControl.Instance.PlaySFX("Computer Wrong", gameObject);
		canTrigger = true;
		cameraZoom.RestoreZoom(zoomSpeed);
	}

	// Called by the beam if the puzzle attempt succeeds;
	public void PuzzleSuccess() {
		_virtualCamera.Follow = _player;
		StartCoroutine(CameraTransitionOutGood());
		OnFinish?.Invoke();
		_playerController.ActivateMovement();
	}

	// Called by the beam if the puzzle attempt fails;
	public void PuzzleFailure() {
		_virtualCamera.Follow = _player;
		StartCoroutine(CameraTransitionOutBad());
		_playerController.ActivateMovement();
	}

	public void SetRoomCode(RoomCode revealRoomCode) {
		this.revealRoomCode = revealRoomCode;
	}
}