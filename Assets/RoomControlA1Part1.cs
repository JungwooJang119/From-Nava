using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlA1Part1 : MonoBehaviour
{

    public GameObject labReport;
    public GameObject door;

    private LabReport lr;

    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        lr = labReport.GetComponent<LabReport>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (lr.GetUnlockStatus()) {
                door.GetComponent<Door>().OpenDoor();
                isActive = false;
            }
        } else {
            Destroy(this.gameObject);
        }
    }
}
