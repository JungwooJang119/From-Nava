using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageToApply) {
        currentHealth -= damageToApply;
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            Destroy(this.gameObject);
        }
    }
}
