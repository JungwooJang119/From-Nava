using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRegularDoor : MonoBehaviour
{
    public GameObject door;
    public bool isToS4;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            door.GetComponent<Door>().OpenDoor();
            if (!isToS4) {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isToS4) {
            door.GetComponent<Door>().OpenDoor();
        }
    }
}
