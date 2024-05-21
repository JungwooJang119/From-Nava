using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<BaseObject>() != null) {
            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        other.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
