using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellCast;
    public LogicScript ls;

    private CircleCollider2D myCollider;
    private Rigidbody2D myRigidbody;
    public string valueDir;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellCast.spellRadius;

        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.isKinematic = true;

        Destroy(this.gameObject, SpellCast.spellLifetime);
        ls = GameObject.FindGameObjectWithTag("GameController").GetComponent<LogicScript>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        valueDir = ls.facingDir;
        if (SpellCast.speed > 0) {
            //Switch
            if (valueDir == "S") {
                transform.Translate(Vector2.down * SpellCast.speed * Time.deltaTime);
            } else if (valueDir == "N") {
                transform.Translate(Vector2.up * SpellCast.speed * Time.deltaTime);
            } else if (valueDir == "W") {
                transform.Translate(Vector2.left * SpellCast.speed * Time.deltaTime);
            } else {
                transform.Translate(Vector2.right * SpellCast.speed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Apply hit particle effects, sfx, spell effects\
        if(other.gameObject.CompareTag("Enemy")) {
            HealthComponent enemyHealth = other.GetComponent<HealthComponent>();
            enemyHealth.TakeDamage(SpellCast.damageAmt);
        }
        //Destroy(this.gameObject);
    }
}
