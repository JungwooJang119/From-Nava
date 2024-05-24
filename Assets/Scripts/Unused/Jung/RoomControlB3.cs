using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB3 : MonoBehaviour
{
    [SerializeField] private GameObject fan1;

    public Fan p1;

    public GameObject door;
    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        p1 = fan1.GetComponent<Fan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (p1.IsRotating) {
                door.GetComponent<Door>().OpenDoor();
                isActive = false;
            }
        }
        if (!isActive) {
            if (!p1.IsRotating) {
                door.GetComponent<Door>().CloseDoor();
                isActive = true;
            }
        }
    }
}
