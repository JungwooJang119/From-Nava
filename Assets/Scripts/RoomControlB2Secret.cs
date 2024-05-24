using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB2Secret : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood[] firewoods;
	[SerializeField] private GameObject[] doorList;
    
    // Start is called before the first frame update
    void Start() {
		firewoods = firewoodController.GetComponentsInChildren<Firewood>();
		foreach (Firewood firewood in firewoods) {
			firewood.OnLitStatusChange += RoomControlA2_OnLitStatusChange;
		}
	}

    private void RoomControlA2_OnLitStatusChange() {
		int firewoodCount = 0;
		foreach (Firewood firewood in firewoods) firewoodCount += firewood.IsPuzzleLit ? 1 : 0;
        if (firewoodCount == firewoods.Length) {
			foreach (Firewood firewood in firewoods) {
				firewood.OnLitStatusChange -= RoomControlA2_OnLitStatusChange;
			} CompleteRoom();
		}
    }


	private void CompleteRoom() {
		foreach (GameObject doorInstance in doorList) {
			doorInstance.GetComponent<Door>().OpenDoor();
		}
        AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
	}
}
