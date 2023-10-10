using System.Collections;
using System.Collections.Generic;
using static RoomLights;
using UnityEngine;
using Cinemachine;

public class RoomControlA3 : MonoBehaviour
{
    [SerializeField] GameObject pressurePlateController;
    [SerializeField] RoomCode revealRoomCode;
    private PressurePlate_Script[] _pressurePlates;
    private bool _canClear = true;
    
    public GameObject B3Chest = null;
    public bool cheat = false;

	private CinemachineVirtualCamera virtualCamera;
	public GameObject cameraTarget;				// The target the camera will move towards.
	public Transform returnToPlayer;
    public GameObject door;

    [SerializeField] private bool isClear = false;
    // [SerializeField] private GameObject spellNotif;

    void Start() {
        _pressurePlates = pressurePlateController.GetComponentsInChildren<PressurePlate_Script>();
        virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        returnToPlayer = PlayerController.Instance.transform;
    }

    void Update() {
        print(isClear);
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
		if (cameraTarget) virtualCamera.Follow = cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
	}

	private void CompleteRoom() {
        AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
		if (!cheat) {
			StartCoroutine(CameraTransitionIn());
			door.GetComponent<Door>().OpenDoor();
		}
        if (B3Chest) B3Chest.SetActive(true);
        ReferenceSingleton.Instance.roomLights.Propagate(revealRoomCode);
        isClear = true;
	}

    public void SetRoomCode(RoomCode revealRoomCode) {
        this.revealRoomCode = revealRoomCode;
    }
}
