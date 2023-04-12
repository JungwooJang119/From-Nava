//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

//Handle input and movement on Player
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] bool isDark;
    [SerializeField] private int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private Text healthText;
    [SerializeField] private float speed = 7f;
    [SerializeField] private DamageFlash damageFlash;

    [SerializeField] private Transform spawn;
    [SerializeField] private Transform rightCast;
    [SerializeField] private Transform leftCast;
    [SerializeField] private Transform upCast;
    [SerializeField] private Transform downCast;
    
    private Vector2 movement;
    private Rigidbody2D rb;
 
    public float collisionOffset = 0.05f;
    public Transform castPoint;

    public Vector2 facingDir;
    private GameObject light;

    public Vector2 FacingDir
    {
        get => facingDir;
    }

    public Animator animator;

    private bool canMove = true;
    private bool canChangeDir = true;

    

    private void Awake() {
        InitializeSingleton();
    }

    private void Start()
    {
        light = this.transform.GetChild(1).gameObject;
        facingDir = Vector2.down;
        castPoint = downCast;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        light.SetActive(isDark);
        playerHealth = maxHealth;
        canMove = true;
        canChangeDir = true;
    }

    private void FixedUpdate() {
        if (canMove) {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
        healthText.text = "Health: " + playerHealth;
    }

    private void OnMove(InputValue movementValue) {
        movement = movementValue.Get<Vector2>();
        ChooseFacingDir();
    }

    private void ChooseFacingDir ()
    {
        if (canChangeDir == false) {
            return;
        }
        if(movement.x > 0) {
            facingDir = Vector2.right;
            castPoint = rightCast;
        }
        if(movement.x < 0) {
            facingDir = Vector2.left;
            castPoint = leftCast;
        }
        if(movement.y > 0) {
            facingDir = Vector2.up;
            castPoint = upCast;
        }
        if(movement.y < 0) {
            facingDir = Vector2.down;
            castPoint = downCast;
        }
        if (movement.magnitude > 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);

            //C: There might be a better way to do this but i could not think of/find one

        } 
        else 
            animator.SetBool("isWalking", false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            // **FIX**
            //Damage taken when melee on enemy
            TakeDamage(1);
            if (playerHealth <= 0) {
                playerHealth = 0;
                StartCoroutine(Die());
            }
        }
    }

    public void TakeDamage(int damage) {
        playerHealth -= damage;
        if (damageFlash != null)
        {
            damageFlash.Flash();
        }
    }

    IEnumerator Die() {
        canMove = false;
        canChangeDir = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
        canChangeDir = true;
        transform.position = spawn.transform.position;
        playerHealth = maxHealth;
    }

    void OnMelee() {
        canMove = false;
        canChangeDir = false;
        animator.SetTrigger("doMelee");
    }

    void ActivateMovement() {
        canMove = true;
        canChangeDir = true;
        ChooseFacingDir();
    }

    void DeactivateMovement() {
        canMove = false;
        canChangeDir = false;
    }

    public void ChangeSpawn(Transform newSpawn) {
        spawn = newSpawn;
    }
}


