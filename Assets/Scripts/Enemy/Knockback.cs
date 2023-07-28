using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null) {
                Vector2 posDiff = enemyRb.transform.position - transform.position;
                posDiff = posDiff.normalized * thrust;
                enemyRb.AddForce(posDiff, ForceMode2D.Impulse);
            }
        }
    }
}
