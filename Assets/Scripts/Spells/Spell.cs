using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellScriptObj spell;
    [SerializeField] private LogicScript logicScript;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 valueDir;

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
        valueDir = logicScript.facingDir;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        transform.Translate(valueDir * spell.speed * Time.deltaTime);
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
