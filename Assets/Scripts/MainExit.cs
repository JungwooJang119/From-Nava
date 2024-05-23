using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainExit : MonoBehaviour
{
    private tranMode tm;

    void Start() {
        tm = GameObject.Find("Transition").GetComponent<tranMode>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<PlayerController>().DeactivateMovement();
            tm.LoadEnding();
        }
    }
}
