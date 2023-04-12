using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellScriptObj spell;
    //[SerializeField] private LogicScript logicScript;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField]private Vector3 rotate;
    [SerializeField]private bool spellActive = false;
    [SerializeField] private bool isChair = false;
    [SerializeField] private bool isWind = false;
    [SerializeField] private bool isPiplup = false;

    public Vector2 direction;


    private void Awake()
    {
        if (!isChair || !isPiplup) {
            transform.Rotate(rotate);
        }

        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;

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
        transform.Translate(Vector3.up * spell.speed * Time.deltaTime);
    }

    public virtual void CastSpell(Vector2 dir) {
        direction = dir;
        if(dir.x < 0)
            rotate = new Vector3(0, 0, 90);
        if (dir.x > 0)
            rotate = new Vector3(0, 0, -90);
        if (dir.y > 0)
            rotate = new Vector3(0, 0, 0);
        if (dir.y < 0)
            rotate = new Vector3(0, 0, 180);

        spellActive = true;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            return;
        }
        
        if (other.gameObject.CompareTag("IceTower")) {
            return;
        }

        if (isChair || isPiplup) {
            return;
        }

        if (isWind) {
            if (other.gameObject.CompareTag("Walls")) {
                Destroy(this.gameObject);
            } else {
                return;
            }
        }

        //Apply hit particle effects, sfx, spell effects\
        if(other.gameObject.CompareTag("Enemy")) {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            enemyHealth.TakeDamage(spell.damageAmt);
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }
}
