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
    [SerializeField] private int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private Text healthText;
    [SerializeField] private float speed = 7f;
    private Vector2 movement;
    private Rigidbody2D rb;
 
    public float collisionOffset = 0.05f;
    public Transform castPoint;
    [SerializeField] private Transform spawn;

    public Vector2 facingDir;

    public Vector2 FacingDir
    {
        get => facingDir;
    }

    private Animator animator;

    private bool canMove = true;

    private void Awake() {
        InitializeSingleton();
    }

    private void Start()
    {
        facingDir = Vector2.down;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = maxHealth;
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
        if(movement.x > 0)
            facingDir = Vector2.right;
        if(movement.x < 0)
            facingDir = Vector2.left;
        if(movement.y > 0)
            facingDir = Vector2.up;
        if(movement.y < 0)
            facingDir = Vector2.down;

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
            TakeDamage(1);
            if (playerHealth <= 0) {
                StartCoroutine(Die());
            }
        }
    }

    public void TakeDamage(int damage) {
        playerHealth -= damage;
        print(playerHealth);
    }

    IEnumerator Die() {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
        transform.position = spawn.transform.position;
        playerHealth = maxHealth;
    }
}


