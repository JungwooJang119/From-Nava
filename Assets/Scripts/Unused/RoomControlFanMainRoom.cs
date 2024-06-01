using System.Linq;
using UnityEngine;

public class RoomControlFanMainRoom : MonoBehaviour {

    [SerializeField] private Fan[] fans;
    [SerializeField] private Door[] doors;

    void Start() {
        foreach (Fan fan in fans) fan.OnBlowingChange += Fan_OnBlowingChange;
    }

    private void Fan_OnBlowingChange() => ToggleDoors(fans.All(fan => fan.IsRotating));
    private void ToggleDoors(bool active) {
        foreach (Door door in doors) {
            if (active) door.OpenDoor();
            else door.CloseDoor();
        }
    }
}
