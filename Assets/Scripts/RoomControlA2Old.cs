using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomControlA2Old : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood_Script[] firewoods;
    private bool canClear = true;

    public bool isClear = false;

    public GameObject A2Chest = null;
    public bool cheat = false;

	private CinemachineVirtualCamera virtualCamera;
	private GameObject cameraTarget;
	private Transform returnToPlayer;
    public GameObject door;
    
    // Start is called before the first frame update
    void Start() {
		firewoods = firewoodController.GetComponentsInChildren<Firewood_Script>();
        virtualCamera = GameObject.Find("Main Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        returnToPlayer = virtualCamera.Follow;
	}

    private void Update() {
        if (!isClear) {
            canClear = true;
            foreach(Firewood_Script _firewood in firewoods) {
                if (!_firewood.GetLit()) {
                    canClear = false;
                    break;
                }
            }
            if (canClear) {
                    CompleteRoom();
            }
        }
    }

	IEnumerator CameraTransitionIn() {
        yield return new WaitForSeconds(0.0f);
		virtualCamera.Follow = cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
		Destroy(this.gameObject);
	}

	private void CompleteRoom() {
		if (!cheat) {
            StartCoroutine(CameraTransitionIn());
			door.GetComponent<Door>().OpenDoor();
            if (A2Chest != null) A2Chest.SetActive(true);
        } else {
			if (A2Chest != null) A2Chest.SetActive(true);
			Destroy(this.gameObject);
		}
	}
}