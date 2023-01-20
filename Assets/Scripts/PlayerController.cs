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

    public string facingDir;
    public int fireDirection = 270;

    public GameObject meleeHB;

    BoxCollider2D meleeCollider;


    //public ContactFilter2D movementFilter;

    //List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    //private GameObject playerObj;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //playerObj = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        meleeCollider = meleeHB.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //change movement to willow like not custom, inefficient
    private void FixedUpdate() {
        /*
        //if mvmt input not 0, move
        if (movementInput != Vector2.zero) {
            //Check for potential collisions
            int count = rb.Cast(
                movementInput, // X and Y values from -1 to 1 that represent the direction from the body to look for collisions
                movementFilter, //Settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // list of collisions to store the found collisions into after the Cast is done
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // amount to cast equal to movement plus offset
            if (count == 0) {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            }
        } 
        */

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
                    //shoot right
                    facingDir = "E";
                    fireDirection = 0;
                } else {
                    // shoot left
                    facingDir = "S";
                    fireDirection = 270;
                }
            } else if (movement.y > movement.x) {
                if (movement.y > 0 && movement.x == 0) {
                    //shoot up
                    facingDir = "N";
                    fireDirection = 90;
                } else {
                    //shoot down
                    facingDir = "W";
                    fireDirection = 180;
                }
            } else {
                if (movement.x > 0) {
                    //shoot right
                    facingDir = "E";
                    fireDirection = 0;
                } else {
                    // shoot left
                    facingDir = "W";
                    fireDirection = 180;
                }
            }
        } else {
            animator.SetBool("isWalking", false);
            }
    }
}
