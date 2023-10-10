using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomLights;
using Cinemachine;

public class RoomControlB1 : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    [SerializeField] private GameObject enemy4;
    [SerializeField] private GameObject enemy5;
    [SerializeField] private GameObject enemy6;

    [SerializeField] private GameObject chest;
    [SerializeField] private bool isC1;
    [SerializeField] private bool isFinal;
    [SerializeField] private GameObject door;

    private CinemachineVirtualCamera virtualCamera;
	public GameObject cameraTarget;				// The target the camera will move towards.
	public Transform returnToPlayer;
    [SerializeField] RoomCode revealRoomCode;
    
    //[SerializeField] private GameObject spellNotif;

    private bool isActive = true;


    // Start is called before the first frame update
    void Start()
    {
        if (isC1) {
            virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
            returnToPlayer = PlayerController.Instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true) {
            if (enemy1 == null && enemy2 == null && enemy3 == null && enemy4 == null && enemy5 == null && enemy6 == null) {
                if (chest != null) {
                    chest.SetActive(true);
                    AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
                }
                if (isFinal) {
                    StartCoroutine(CameraTransitionIn());
                    AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
                    door.GetComponent<Door>().OpenDoor();
                }
                // spellNotif.SetActive(true);
                // StartCoroutine(DurationTime());
                isActive = false;
                if (isC1) {
                    ReferenceSingleton.Instance.roomLights.Propagate(revealRoomCode);
                    StartCoroutine(CameraTransitionIn());
                    AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
                    door.GetComponent<Door>().OpenDoor();
                }
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.0f);
		if (cameraTarget) virtualCamera.Follow = cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
	}

    public void SetRoomCode(RoomCode revealRoomCode) {
        this.revealRoomCode = revealRoomCode;
    }

}
