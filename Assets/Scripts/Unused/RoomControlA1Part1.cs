using UnityEngine;

public class RoomControlA1Part1 : MonoBehaviour {

    public GameObject labReport;
    public GameObject door;

    // Start is called before the first frame update
    void Start() {
        // Suscribe to Lab Report event;
        labReport.GetComponent<LabReport>().OnReportRead += RoomControlA1Part1_OnReportRead;
    }

    private void RoomControlA1Part1_OnReportRead() {
        door.GetComponent<Door>().OpenDoor();
        ReferenceSingleton.Instance.roomLights.Propagate(RoomLights.RoomCode.A1_2);
        Destroy(this.gameObject);
    }
}