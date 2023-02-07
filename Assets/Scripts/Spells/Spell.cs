using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellScriptObj spell;
    [SerializeField] private LogicScript logicScript;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField]private Vector2 direction;
    [SerializeField]private bool spellActive = false;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        Destroy(this.gameObject, spell.lifetime); 
    }

    protected virtual void Update()
    {
        if(spellActive)
            MoveSpell();
    }

    private void MoveSpell()
    {
        transform.Translate(direction * spell.speed * Time.deltaTime);
    }

    public virtual void CastSpell(Vector2 dir) {
        direction = dir;
        spellActive = true;
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
