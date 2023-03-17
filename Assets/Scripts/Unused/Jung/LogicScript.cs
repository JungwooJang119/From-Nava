using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private Text healthText;

    [SerializeField] private Spell _spell;
    [SerializeField] private Spell _chairSpell;
    [SerializeField] private Melee _melee;
    [SerializeField] private Transform castPoint;
    [SerializeField] private IceTowerController iceTower;

    [SerializeField] private float maxMagic = 100f;
    [SerializeField] private float currMagic;
    [SerializeField] private float refillRate = 2f;
    [SerializeField] private float refillLag = 1f; //1 second
    [SerializeField] private float currMagicRefillTimer;
    [SerializeField] private float castInterval = 0.25f;
    [SerializeField] private float currCastTimer;

    private bool castingMagic = false;

    [SerializeField] private string patternActive = "";
    private int currLines;

    public Vector2 facingDir;
    private PlayerController playerController;
    [SerializeField] private ManaMeter manaMeter;

    void Awake() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHealth = maxHealth;
        currMagic = maxMagic;
        manaMeter.SetMaxMana(maxMagic);
    }

    /*
        have the player take damage
        TODO: maybe separate health into another script?
    */
    public void TakeDamage(int damage) {
        playerHealth -= damage;
    }

    public int GetHealth() {
        return playerHealth;
    }
    /*
    @desc Builds string holding pattern of spell currently drawn and Casts spell when pattern matches
    @params string newNode The string name of the button / node clicked on in UI
    @return 
    */
    public void setString(string newNode) {
        patternActive += newNode;
        if (newNode == "9") {
            if (patternActive == "59") { //Fireball Spell
                bool hasEnoughMana = ((currMagic - _spell.spell.manaCost) >= 0f);
                if (!castingMagic && hasEnoughMana) {
                    castingMagic = true;
                    currMagic -= _spell.spell.manaCost;
                    currCastTimer = 0;
                    currMagicRefillTimer = 0;
                    CastSpell();
                }    
            }
            if (patternActive == "269") {
                //demonstrate disabling ice tower
                iceTower.disableTower = true; 
            }
            // if (patternActive == "259") {
            //     CastChairSpell();
            // }
            if (patternActive == "9") {
                CastMelee();
            }
            patternActive = "";
        }
    }

    private void Update() {
        manaMeter.SetManaMeter(currMagic);
        if (castingMagic) {
                currCastTimer += Time.deltaTime;
                if (currCastTimer > castInterval) {
                    castingMagic = false;
                }
            }

            if (currMagic < maxMagic && (!castingMagic)) {
                currMagicRefillTimer += Time.deltaTime;
                if (currMagicRefillTimer > refillLag) {
                    currMagic += refillRate * Time.deltaTime;            
                    if (currMagic > maxMagic) {
                        currMagic = maxMagic;
                    }
                }
            }   
    }
/*
    @desc Sets facing direction of player correctly and casts spell at current facing direction
    @params 
    @return 
    */
    private void CastSpell() {
        facingDir = PlayerController.Instance.FacingDir;
        Instantiate(_spell, castPoint.position, Quaternion.identity);
    }
    /*

    */
    private void CastMelee() {
        _melee.activateMelee();
    }

// /*
//     @desc Sets facing direction of player correctly and creates a chair at current facing direction.
//     */
//     private void CastChairSpell() {
//         facingDir = playerController.facingDir;
//         Vector3 newVector = new Vector3(2 * Mathf.Cos(Mathf.PI * facingDir / 180), 2 * Mathf.Sin(Mathf.PI * facingDir / 180), 0);
//         Instantiate(_chairSpell, castPoint.position + newVector, Quaternion.identity);
//     }

    public int getCurrLines() {
        return currLines;
    }

    public void setCurrLines(int amt) {
        currLines += amt;
    }
}
