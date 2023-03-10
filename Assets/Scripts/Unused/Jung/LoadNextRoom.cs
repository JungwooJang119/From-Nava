using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextRoom : MonoBehaviour
{
    [SerializeField] private Transform spawn;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.transform.position = spawn.transform.position;
        }
    }
}
