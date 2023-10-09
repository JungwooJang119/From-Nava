using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB2Secret : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood_Script[] firewoods;
	private int firewoodCount = 0;
	[SerializeField] private GameObject[] doorList;
    
    // Start is called before the first frame update
    void Start() {
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


	private void CompleteRoom() {
		foreach (GameObject doorInstance in doorList) {
			doorInstance.GetComponent<Door>().OpenDoor();
		}
        AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
	}
}
