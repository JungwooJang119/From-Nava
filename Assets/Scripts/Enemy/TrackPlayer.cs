using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    private bool playerIn;
    // Start is called before the first frame update

    void Start() {
        playerIn = false;
    }
    void OnTriggerExit2D(Collider2D other) {
        playerIn = false;
    }

    void OnTriggerStay2D(Collider2D other) {
        playerIn = true;
    }

    public bool GetPlayerIn() {
        return playerIn;
    }
}
