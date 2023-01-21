using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    private float currHealth;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        currHealth -= damage;
        //Debug.Log(currHealth);
        if (currHealth <= 0) {
            Destroy(this.gameObject);
        }
    }
}
