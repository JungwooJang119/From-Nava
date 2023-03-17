//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Handle input and movement on Player
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] bool isDark;
    [SerializeField] private float speed = 7f;
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

    private Animator animator;

    private void Awake() {
        InitializeSingleton();

    }

    private void Start()
    {
        light = this.transform.GetChild(1).gameObject;
        facingDir = Vector2.down;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        light.SetActive(isDark);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
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

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("OnCollisionEnter2D");
        
    }
}


