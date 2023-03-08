using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFansLaser : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        print("Fuck");
        if (other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(1);
        }
    }
}
