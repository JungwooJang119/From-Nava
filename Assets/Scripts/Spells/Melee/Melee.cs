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

    //how long should hitbox last, in frames
    private float duration = 15f;
    //how much damage the melee does
    private float damage = 5f;
    //timer
    private float timeSinceLast;
    // Start is called before the first frame update
    void Start()
    {
        //loads box collider and makes it a trigger
        //boxCollider = GetComponent<BoxCollider2D>();
        //boxCollider.isTrigger = false;

        //loads all four directional melee hitboxes
        hitboxN = GetComponent<BoxCollider2D>();
        hitboxN.isTrigger = false;
        hitboxE = GetComponent<BoxCollider2D>();
        hitboxE.isTrigger = false;
        hitboxS = GetComponent<BoxCollider2D>();
        hitboxS.isTrigger = false;
        hitboxW = GetComponent<BoxCollider2D>();
        hitboxW.isTrigger = false;

        //logicScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<LogicScript>();
        //valueDir = logicScript.facingDir;
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
            enemyHealth.TakeDamage(damage);
        }
        //Destroy(this.gameObject);
    }
    /*
    public void melee()
    {
        Vector2 meleeVector = transform.position;
        valueDir = logicScript.facingDir;
        switch (valueDir)
        {
            case 0:
                meleeVector = meleeVector + Vector2.right;
                break;
            case 90:
                meleeVector = meleeVector + Vector2.up;
                break;
            case 180:
                meleeVector = meleeVector + Vector2.left;
                break;
            case 270:
                meleeVector = meleeVector + Vector2.down;
                break;
        }
        //Instantiate(MeleeCollider, meleeVector, Quaternion.identity);
    }
    */

    public void activateMelee() {
        //get look direction and activate correct hitbox
        /*
        switch (facingDir)
        {
            case 0:
                hitboxE.isTrigger = true;
                break;
            case 90:
                hitboxN.isTrigger = true;
                break;
            case 180:
                hitboxW.isTrigger = true;
                break;
            case 270:
                hitboxS.isTrigger = true;
                break;
        }
        timeSinceLast = duration; //start the countdown!
        */
    }

    private void deactivateMelee() {
        //deactivating all hitboxes
        hitboxN.isTrigger = false;
        hitboxE.isTrigger = false;
        hitboxS.isTrigger = false;
        hitboxW.isTrigger = false;
    }
}

/*
notes on melee collider:

box collider that ALWAYS follows the player.
isTrigger = false, by default

Update() {
  [follow player around, account for vision]
  [decrement timer]
}

melee() {
  isTrigger = true;
  [start a timer for active hitbox]
}
*/