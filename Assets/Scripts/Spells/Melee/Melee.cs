/*
I'm basically copying Spell.cs and seeing what happens.
Leaving comments to see what the code probably does as I transcribe it.
Anyway, expect maybe thirty extra lines. -Ray/altamoth
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public Transform player;

    //[SerializeField] private LogicScript logicScript;
    //[SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private BoxCollider2D hitboxN;
    [SerializeField] private BoxCollider2D hitboxE;
    [SerializeField] private BoxCollider2D hitboxS;
    [SerializeField] private BoxCollider2D hitboxW;

    private float valueDir;

    //how much damage the melee does
    public int damage;
    //timer
    private float timeSinceLast;
    // Start is called before the first frame update
    void Start()
    {
        hitboxN = GetComponent<BoxCollider2D>();
        hitboxN.isTrigger = true;
        hitboxE = GetComponent<BoxCollider2D>();
        hitboxE.isTrigger = true;
        hitboxS = GetComponent<BoxCollider2D>();
        hitboxS.isTrigger = true;
        hitboxW = GetComponent<BoxCollider2D>();
        hitboxW.isTrigger = true;
        timeSinceLast = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLast == 0) { //tick down the timer
            deactivateMelee();
        } else if (timeSinceLast > 0) {
            timeSinceLast = timeSinceLast - 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Apply hit particle effects, sfx, spell effects - copied from Spell.cs

        //checks if melee hits enemy
        if(other.gameObject.CompareTag("Enemy")) {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            if (enemyHealth.isIceTower) {
                return;
            }
            enemyHealth.TakeDamage(damage, other.gameObject);
        }
        if (other.gameObject.CompareTag("EnemyProjectile")) {
            Destroy(other.gameObject);
        }
    }

    private void deactivateMelee() {
        //deactivating all hitboxes
        hitboxN.isTrigger = false;
        hitboxE.isTrigger = false;
        hitboxS.isTrigger = false;
        hitboxW.isTrigger = false;
    }
}