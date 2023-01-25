//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Handle input and movement on Player
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    private Vector2 movement;
    private Rigidbody2D rb;
 
    public float collisionOffset = 0.05f;

    public float facingDir;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue movementValue) {
        movement = movementValue.Get<Vector2>();
        if (movement.x != 0 || movement.y != 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);
            if (movement.x > movement.y) {
                if (movement.x > 0) {
                    facingDir = 0;
                } else {
                    facingDir = 270;
                }
            } else if (movement.y > movement.x) {
                if (movement.y > 0 && movement.x == 0) {
                    facingDir = 90;
                } else {
                    facingDir = 180;
                }
            } else {
                if (movement.x > 0) {
                    facingDir = 0;
                } else {
                    facingDir = 180;
                }
            }
        } else {
            animator.SetBool("isWalking", false);
        }
    }
}
