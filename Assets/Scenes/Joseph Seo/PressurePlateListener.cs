using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateListener : Listener
{
    [SerializeField] GameObject pressurePlateController;            // GameObject that contains all necessary PressurePlates
    [Tooltip("Only add a group of doors to this if they are a part of the puzzle rather than the reward.")]
    // Filling this next field out results in the puzzle appearing as if 
    [SerializeField] GameObject DoorController;                     // GameObject for the doors these pressure plates will toggle. Note: doing this will disable this 
                                                                    // components usage for Room Control. 

    private PressurePlate_Script[] _pressurePlates;                 // The actual list of PressurePlate components
    private Door[] _doors;                                          // The door components
    private bool previousDoorStatus = false;                        // The previous status of the controlled doors. Logic described in TogglePuzzle

    void Awake() {
        // Grab all the pressure plates and subscribe to whenever they change (ie get pressed or unpressed)
        _pressurePlates = pressurePlateController.GetComponentsInChildren<PressurePlate_Script>();
        foreach (PressurePlate_Script plate in _pressurePlates) {
            plate.OnPressedStatusChange += OnStatusChange;
        }

        // If DoorController == null, then this will be used as a normal listener and Invoke OnListen to return the status of this component.
        // If DoorController != null, then this component will now toggle the doors in DoorController. 
        if (DoorController != null) {
            _doors = DoorController.GetComponentsInChildren<Door>();
        }
    }

    protected override void OnStatusChange() {
        // If DoorController is null, proceed as a normal listener and check that every pressure plate is pressed.
        if (DoorController == null) {
            foreach(PressurePlate_Script plate in _pressurePlates) {
                if (!plate.GetIsPressed()) {
                    return;
                }
            }
            status = true;
            OnListen?.Invoke();
        }
        
    
        else {
            TogglePuzzle();
        }
    }

    private void TogglePuzzle() {
        // Assume that no plates are pressed and check every plate. If one is pressed, then the doors must open, meaning isOpen = true.
        bool isOpen = false;
        foreach (PressurePlate_Script plate in _pressurePlates) {
            if (plate.GetIsPressed()) {
                isOpen = true; 
                break;
            }
        }

        // If these two values are not the same, then the state of the doors has changed.
        // If previousDoorStatus = false, and isOpen = true, then the doors were closed and now must open. Toggle the doors to open.
        // If previousDoorStatus = true, and isOpen = false, then the doors were open but no plates are presed. Toggle the doors to close.
        // If previousDoorStatus = true, and isOpen = true, then the doors are open and must stay open. Do nothing. Same applies to false.
        if (isOpen != previousDoorStatus) {
            previousDoorStatus = isOpen;
            foreach (Door door in _doors) {
                door.FlipDoor();
            }
        }


    }

}
