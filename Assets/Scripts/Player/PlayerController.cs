//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Handle input and movement on Player
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float speed = 7f;
    private Vector2 movement;
    private Rigidbody2D rb;
 
    public float collisionOffset = 0.05f;

    private Vector2 facingDir;
    public Vector2 FacingDir => facingDir;

    private Animator animator;

    private void Awake() {
        InitializeSingleton();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue movementValue) {
        movement = movementValue.Get<Vector2>();
    }

    private void ChooseFacingDir (Vector2 movement)
    {
        if (movement.magnitude > 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);

            //C: There might be a better way to do this but i could not think of/find one
            if (movement.x > movement.y) {
                if (movement.x > 0) 
                    facingDir = Vector2.right;
                else 
                    facingDir = Vector2.down;
            } else if (movement.y > movement.x) {
                if (movement.y > 0 && movement.x == 0) 
                    facingDir = Vector2.up;
                else 
                    facingDir = Vector2.down;
            } else {
                if (movement.x > 0) 
                    facingDir = Vector2.right;
                else 
                    facingDir = Vector2.down;
            }
        } 
        else 
            animator.SetBool("isWalking", false);
    }
}


