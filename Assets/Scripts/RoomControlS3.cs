using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomControlS3 : MonoBehaviour
{
    [SerializeField] private GameObject sensor;
    private CinemachineVirtualCamera virtualCamera;
	public GameObject cameraTarget;				// The target the camera will move towards.
	public Transform returnToPlayer;
    public GameObject chest = null;

    private bool hasCompleted;

    void Start() {
        virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        returnToPlayer = PlayerController.Instance.transform;
    }

    void Update()
    {
        if (sensor.GetComponent<CopySensor>().GetStatus() && !hasCompleted) {
            CompleteRoom();
            hasCompleted = true;
        }
    }

    private void CompleteRoom() {
        AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
		StartCoroutine(CameraTransitionIn());
        chest.SetActive(true);
	}

    IEnumerator CameraTransitionIn() {
		yield return new WaitForSeconds(0.0f);
		if (cameraTarget) virtualCamera.Follow = cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        virtualCamera.Follow = returnToPlayer;
        Destroy(this.gameObject);
	}
}
