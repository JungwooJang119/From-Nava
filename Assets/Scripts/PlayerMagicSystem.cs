using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMagicSystem : MonoBehaviour
{
    //[SerializeField] private Spell _spell;
    /*
    public Spell _spell;
    public Transform castPoint;

    [SerializeField] private float maxMagic = 100f;
    [SerializeField] private float currMagic;
    [SerializeField] private float magicRefillRate = 2f;
    [SerializeField] private float refillRateDelay = 1f; //1 second
    private float currMagicRefillTimer;
    [SerializeField] private float magicCastInterval = 0.25f;
    //[SerializeField] private Transform castPoint;

    [SerializeField] private float currCastTimer;

    private bool castingMagic = false;

    private PlayerInput _input;


    private void Awake() {
        _input = GetComponent<PlayerInput>();
        currMagic = maxMagic;
    }

    private void OnEnable()
    {
        
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        
    }
/*
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        bool isSpellCastHeldDown = playerControls.Player.SpellCast.ReadValue<float>() > 0.1;
        if (!castingMagic && isSpellCastHeldDown) {
            Debug.Log("HEllo");
        }
    } */

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    /*
    private void Update()
    {
        bool hasReqMagic = ((currMagic - _spell.SpellToCast.magicCost) >= 0f);
        if (_input.actions["Spell Cast"].IsPressed()) {
            if (!castingMagic && hasReqMagic) {
                castingMagic = true;
                currMagic -= _spell.SpellToCast.magicCost;
                currCastTimer = 0;
                currMagicRefillTimer = 0;
                CastSpell();
            }
        }

        if (castingMagic) {
            currCastTimer += Time.deltaTime;
            if (currCastTimer > magicCastInterval) {
                castingMagic = false;
            }
        }

        if (currMagic < maxMagic && (!castingMagic) && (!(_input.actions["Spell Cast"].IsPressed()))) {
            currMagicRefillTimer += Time.deltaTime;
            if (currMagicRefillTimer > refillRateDelay) {
                currMagic += magicRefillRate * Time.deltaTime;            
                if (currMagic > maxMagic) {
                    currMagic = maxMagic;
                }
            }
        }
    }
    

    void OnSpellCast() {
        if (!castingMagic) {
            castingMagic = true;
            currentCastTimer = 0;
            Debug.Log("Casting spell");
        }

        if (castingMagic) {
            currentCastTimer += Time.deltaTime;
            if (currentCastTimer > magicCastInterval) {
                castingMagic = false;
            }
        }

    }
    void CastSpell() {
        Instantiate(_spell, castPoint.position, Quaternion.identity);
    }
    */
}
