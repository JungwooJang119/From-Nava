using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomLights;
using Cinemachine;

public class RoomControl : MonoBehaviour {
    [SerializeField] private List<Listener> puzzleListeners;        // A list of Listeners that RoomControl will receive signals to check.
    [SerializeField] private List<GameObject> rewards;              // A list of rewards that will be given due to the puzzle being solved.
    [SerializeField] private bool PlaySound = true;                 // If a jingle should be played or not on solve. Defaults to true.
    [SerializeField] private GameObject cameraTarget;				// The target the camera will move towards. If null, the camera will not pan.
    [SerializeField] RoomCode revealRoomCode;                       // Which next room (in the same sector) should be revealed.

    private List<RewardObject> rewardObjectComponents = new List<RewardObject>();   // The interface implemented by rewards to call DoReward
    private CinemachineVirtualCamera virtualCamera;                                 // The virtual camera for camera pans


    void Start() {
        // Grabs the virtual camera
        if (cameraTarget != null) virtualCamera = ReferenceSingleton.Instance.mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();

        // Starts receiving puzzle updates from each listener
        foreach (Listener listener in puzzleListeners) {
            listener.OnListen += UpdateStatus;
        }

        // Grabs the RewardObject component of each reward to call DoReward later
        foreach (GameObject reward in rewards) {
            rewardObjectComponents.Add(reward.GetComponent<RewardObject>());
        }
    }

    // When receiving an event from a listener, check every listener and see if each listener has been solved or not
    // If so, complete the puzzle, giving out rewards and transitioning.
    private void UpdateStatus() {
        foreach (Listener listener in puzzleListeners) {
            if (!listener.CheckStatus()) return;
        }
        PuzzleCompletion();
    }

    private void PuzzleCompletion() {
        if (PlaySound) AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
        foreach (RewardObject obj in rewardObjectComponents) {
            obj.DoReward();
        }
        if (cameraTarget != null) {
            StartCoroutine(CameraTransitionIn());
        }
        ReferenceSingleton.Instance.roomLights.Propagate(revealRoomCode);

        foreach (Listener listener in puzzleListeners) {
            listener.OnListen -= UpdateStatus;
        }
    }

    // Camera transition script.
    IEnumerator CameraTransitionIn() {
        yield return new WaitForSeconds(0.0f);
        if (cameraTarget) virtualCamera.Follow = cameraTarget.transform;
        yield return new WaitForSeconds(2f);
        virtualCamera.Follow = PlayerController.Instance.transform;
    }

}