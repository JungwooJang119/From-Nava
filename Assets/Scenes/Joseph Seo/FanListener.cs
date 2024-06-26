using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanListener : Listener
{
    [SerializeField] private GameObject fans;               // GameObject containing all the fans necessary for this door or puzzle
    [SerializeField] private GameObject puzzleDoors;        // GameObject for the doors these fans will toggle. Note: doing this will disable this 
                                                            // components usage for Room Control. 
    [SerializeField] private bool isFlipped = false;        // Boolean for C1, where the fans must be off to open the door

    private Fan[] _fans;                                    // Actual Fan components
    private Door[] _doors;                                  // Actual Door components that the fans will toggle.
    private bool previousDoorStatus;                        // The previous status of the doors. Logic described in TogglePuzzle

    void Awake() {
        // Grab all the fans and subscribe to their state changes.
        _fans = fans.GetComponentsInChildren<Fan>();
        foreach (Fan fan in _fans) fan.OnBlowingChange += OnStatusChange;
        // If puzzleDoors is not null, get all the doors as well.
        if (puzzleDoors != null) {
            _doors = puzzleDoors.GetComponentsInChildren<Door>();
        }
    }

    // TODO: FIX THIS; FIND SOME BETTER LOGIC OR SOMETHING
    protected override void OnStatusChange() {
        if (!isFlipped) {
            bool allFansRotating = true;
            foreach (Fan fan in _fans) {
                if (!fan.IsRotating) {
                    allFansRotating = false;
                    break;
                }
            }
            if (puzzleDoors == null) {
                if (!allFansRotating) return;
                status = true;
                OnListen?.Invoke();
            } else {
                if (allFansRotating != previousDoorStatus) {
                    previousDoorStatus = allFansRotating;
                    foreach(Door door in _doors) {
                        door.FlipDoor();
                    }
                }
            }
        } 
        
        else {
            bool allFansRotating = false;
            foreach (Fan fan in _fans) {
                if (fan.IsRotating) {
                    allFansRotating = true;
                    break;
                }
            }
            if (puzzleDoors == null) {
                if (allFansRotating) return;
                status = true;
                OnListen?.Invoke();
            } else {
                if (allFansRotating == previousDoorStatus) {
                    previousDoorStatus = !allFansRotating;
                    foreach (Door door in _doors) {
                        door.FlipDoor();
                    }
                }
            }
        }
    }

}
