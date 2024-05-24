using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell_Behavior : MonoBehaviour
{
    [SerializeField] private float speedReduce;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            print("Speed reduction applied");
            other.gameObject.GetComponent<FireGoombaController>().speed /= speedReduce; // change this once the enemy has a speed variable
        }
    }
}
