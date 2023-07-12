using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyMelee : MonoBehaviour
{
    public PlayerController player;
    private Vector3 dir;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            player.TakeDamage(2);
        }
        dir = player.gameObject.GetComponent<Transform>().position - transform.position;
        if (player.playerHealth > 0) {
            player.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(2000*dir);
        }

        //fires wind blasts based on direction (only three of them away from big guy)
    }
}
