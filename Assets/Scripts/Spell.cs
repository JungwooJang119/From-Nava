using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellScriptObj spell;
    [SerializeField] private LogicScript logicScript;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Rigidbody2D rb;

    private string valueDir;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        Destroy(this.gameObject, spell.lifetime); 
        logicScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<LogicScript>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        valueDir = logicScript.facingDir;
        if (spell.speed > 0) {
            if (valueDir == "S") {
                transform.Translate(Vector2.down * spell.speed * Time.deltaTime);
            } else if (valueDir == "N") {
                transform.Translate(Vector2.up * spell.speed * Time.deltaTime);
            } else if (valueDir == "W") {
                transform.Translate(Vector2.left * spell.speed * Time.deltaTime);
            } else {
                transform.Translate(Vector2.right * spell.speed * Time.deltaTime);
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
            Enemy enemyHealth = other.GetComponent<Enemy>();
            enemyHealth.TakeDamage(spell.damageAmt);
        }
        //Destroy(this.gameObject);
    }
}
