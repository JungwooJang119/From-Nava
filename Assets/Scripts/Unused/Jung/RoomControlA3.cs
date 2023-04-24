using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomControlA3 : MonoBehaviour
{
    [SerializeField] GameObject pressurePlateController;
    private PressurePlate_Script[] _pressurePlates;
    private bool _canClear = true;
    
    public GameObject B3Chest = null;
    public bool cheat = false;

    public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
	public GameObject _cameraTarget;				// The target the camera will move towards.
	public Transform _returnToPlayer;
    public GameObject door;

    [SerializeField] private bool isClear = false;
    // [SerializeField] private GameObject spellNotif;

    void Start() {
        _pressurePlates = pressurePlateController.GetComponentsInChildren<PressurePlate_Script>();
        _virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
    }

    void Update() {
        if (!isClear) {
            _canClear = true;
            foreach (PressurePlate_Script plate in _pressurePlates) {
                if (!plate.GetIsPressed()) {
                    _canClear = false;
                    break; 
                }
            }
            if (_canClear) CompleteRoom();
        }
    }

    IEnumerator DurationTime() {
		yield return new WaitForSeconds(5f);
		//spellNotif.SetActive(false);
	}

    IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.0f);
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        _virtualCamera.Follow = _returnToPlayer;
	}

	private void CompleteRoom() {
		if (!cheat) {
			StartCoroutine(CameraTransitionIn());
			door.GetComponent<Door>().OpenDoor();
		}
        if (B3Chest) B3Chest.SetActive(true);
		isClear = true;
	}
}
