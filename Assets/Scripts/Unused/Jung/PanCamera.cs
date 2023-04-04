using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PanCamera : MonoBehaviour
{

    // Variables for camera transition
	public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
    public Camera MainCamera;
	public GameObject _cameraTarget;				// The target the camera will move towards.
	private Transform _returnToPlayer;				// Stores the original follow that the camera shall return to.

    // Variables to calculate whether the player is in range, following Grace's script;
	private Transform _player;
	private float _currentDistance;

    private bool isActive = false;

    void Start() {
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
	}

    // Utilizes Grace's Text Pop Script to check if the player is in range for interaction;
	// void Update() {
	// 	if ((Input.GetKeyDown(KeyCode.Space)) && !isActive) {
    //         isActive = true;
	// 		StartCoroutine(CameraTransitionIn());
	// 	}
	// }

	public void startPan() {
		isActive = true;
		StartCoroutine(CameraTransitionIn());
	}


    // Corouting to start the camera transtion;
	IEnumerator CameraTransitionIn() {
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        StartCoroutine(CameraTransitionOut());
	}

	IEnumerator CameraTransitionOut() {
		yield return new WaitForSeconds(0.5f);
        _virtualCamera.Follow = _player.transform;
        isActive = false;
	}
}
