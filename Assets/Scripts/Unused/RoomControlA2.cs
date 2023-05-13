using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomControlA2 : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood_Script[] firewoods;
	private int firewoodCount = 0;

    public GameObject A2Chest = null;
    public bool cheat = false;

	private CinemachineVirtualCamera virtualCamera;
	private GameObject cameraTarget;
	private Transform returnToPlayer;
    public GameObject door;
    
    // Start is called before the first frame update
    void Start() {
		cameraTarget = A2Chest ? A2Chest : door;
		virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
		returnToPlayer = PlayerController.Instance.transform;

		firewoods = firewoodController.GetComponentsInChildren<Firewood_Script>();
		foreach (Firewood_Script firewood in firewoods) {
			firewood.OnLitStatusChange += RoomControlA2_OnLitStatusChange;
			if (firewood.GetLit()) firewoodCount++;
		}
	}

    private void RoomControlA2_OnLitStatusChange(int change) {
		firewoodCount += change;
        if (firewoodCount == firewoods.Length) {
			foreach (Firewood_Script firewood in firewoods) {
				firewood.OnLitStatusChange -= RoomControlA2_OnLitStatusChange;
			} CompleteRoom();
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
