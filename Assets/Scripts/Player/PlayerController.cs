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

    private bool isPushed;
    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;

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
        if (isPushed) {
            PushTranslate();
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
        if (other.gameObject.CompareTag("EnemyProjectile")) {
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
        if (canMove) {
			AudioControl.Instance.PlaySFX("Melee Cast", gameObject, 0.2f);
			canMove = false;
			canChangeDir = false;
			animator.SetTrigger("doMelee");
		}
    }

    public void ActivateMovement() {
        canMove = true;
        canChangeDir = true;
        ChooseFacingDir();
    }

    public void DeactivateMovement() {
        canMove = false;
        canChangeDir = false;
    }

    public void ChangeSpawn(Transform newSpawn) {
        spawn = newSpawn;
    }

    //adding push behavior for spikes
    public void Push(Vector2 dir, float dist, float spd) {
        canMove = false;
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
    }

    public void PushTranslate() {
        if (pushDist <= 0) {
            canMove = true;
            isPushed = false;
        } else {
            transform.Translate(pushDir * pushSpd * Time.deltaTime);
            pushDist -= (pushDir *  pushSpd * Time.deltaTime).magnitude;
        }
    }

    public bool GetPushed() {
        return isPushed;
    }
}


