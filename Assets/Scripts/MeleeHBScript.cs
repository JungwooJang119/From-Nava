using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHBScript : MonoBehaviour
{
    public BoxCollider2D meleeCollider;

    void Start() {
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy")) {
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            enemyHealth.TakeDamage(10);
        }
    }
}
