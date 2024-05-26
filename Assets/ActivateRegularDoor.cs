using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRegularDoor : MonoBehaviour
{
    public GameObject door;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            door.GetComponent<Door>().OpenDoor();
            Destroy(this.gameObject);
        }
    }
}
