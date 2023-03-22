using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Collider2D meleeCollider;
    [SerializeField] private float meleeDamage = 10f;

    private void Start() {
        if (meleeCollider == null) {
            print("No Melee Collider!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy") {
            other.collider.SendMessage("OnMeleeHit", meleeDamage);
        }
    }
}