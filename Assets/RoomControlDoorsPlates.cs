using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlDoorsPlates : MonoBehaviour
{
    //[SerializeField] private PressurePlate_Script _pressurePlate;
    [SerializeField] GameObject pressurePlateController;
    [SerializeField] GameObject doorController;
    private PressurePlate_Script[] _pressurePlates;
    private Door[] _doors;

    private bool _isClear = false;

    private bool hasChecked = false;

    void Start() {
        _pressurePlates = pressurePlateController.GetComponentsInChildren<PressurePlate_Script>();
        _doors = doorController.GetComponentsInChildren<Door>();
    }

    void Update() {
        foreach (PressurePlate_Script plate in _pressurePlates) {
            if (plate.GetIsPressed() && !hasChecked) {
                _isClear = true;
                foreach (Door door in _doors) {
                    if (door.isOpen) {
                        StartCoroutine(DoorClose(door));
                    } else {
                        StartCoroutine(DoorOpen(door));
                    }
                    // door.OpenDoor();
                }
                hasChecked = true;
                break;
            } else if (_isClear == true && !hasChecked) {
                _isClear = false;
                foreach (Door door in _doors) {
                    if (door.isOpen) {
                        StartCoroutine(DoorClose(door));
                    } else {
                        StartCoroutine(DoorOpen(door));
                    }
                    // door.CloseDoor();
                }
                hasChecked = true;
                break;
            }
        }
        int count = 0;
        foreach (PressurePlate_Script plate in _pressurePlates) {
            if (plate.GetIsPressed() == false) {
                count++;
            }
        }
        if (count == _pressurePlates.Length) {
            hasChecked = false;
        }
    }

    IEnumerator DoorOpen(Door door) {
        yield return new WaitForSeconds(0.0f);
        door.GetComponent<Door>().OpenDoor();
    }

    IEnumerator DoorClose(Door door) {
        yield return new WaitForSeconds(0.0f);
        door.GetComponent<Door>().CloseDoor();
    }
}
