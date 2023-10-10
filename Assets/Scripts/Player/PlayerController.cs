//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//Handle input and movement on Player
public class PlayerController : Singleton<PlayerController>, IDamageable, IPushable
{
    [SerializeField] bool isDark;
    public int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed = 7f;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private DealthDissolveShader dissolveShader;

    public Transform spawn;
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

    public bool canMove = true;
    private bool canChangeDir = true;

    private PlayerInput input;

    [SerializeField] private float iFrameTime;
    private float currIFrameTime;
    private bool canBeDamaged;
    private bool hasDusted;
    public ParticleSystem dust;

    [SerializeField] private GameObject[] arrows;


    private void Awake() {
        InitializeSingleton();
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        light = this.transform.GetChild(0).gameObject;
        facingDir = Vector2.down;
        arrows[3].SetActive(true);
        castPoint = downCast;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDark = (spawn.gameObject.tag == "DarkRoom");
        light.SetActive(isDark);
        playerHealth = maxHealth;
        canMove = true;
        canChangeDir = true;
        canBeDamaged = true;
        currIFrameTime = iFrameTime;
    }

    private void FixedUpdate() {
        if (input.actions["MainMenu"].IsPressed()) {
            SceneManager.LoadScene(0);
        }
        if (canMove) {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
        if (isPushed) {
            PushTranslate();
        }
        if (playerHealth > 0 && currIFrameTime >= iFrameTime) {
            canBeDamaged = true;
        } else {
            canBeDamaged = false;
            currIFrameTime += Time.deltaTime;
        }
    }

    private void OnMove(InputValue movementValue) {
        movement = movementValue.Get<Vector2>();
        ChooseFacingDir();
        hasDusted = false;
        if (!hasDusted) { //constantly playing
            hasDusted = true;
            dust.Play();
        }
    }

    private void ChooseFacingDir ()
    {
        if (canChangeDir == false) {
            return;
        }
        if(movement.x > 0) {
            facingDir = Vector2.right;
            castPoint = rightCast;
            SetAllArrowsFalse();
            arrows[0].SetActive(true);
        }
        if(movement.x < 0) {
            facingDir = Vector2.left;
            castPoint = leftCast;
            SetAllArrowsFalse();
            arrows[1].SetActive(true);
        }
        if(movement.y > 0) {
            facingDir = Vector2.up;
            castPoint = upCast;
            SetAllArrowsFalse();
            arrows[2].SetActive(true);
        }
        if(movement.y < 0) {
            facingDir = Vector2.down;
            castPoint = downCast;
            SetAllArrowsFalse();
            arrows[3].SetActive(true);
        }
        if (movement.magnitude > 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);
        } 
        else 
            animator.SetBool("isWalking", false);
    }

    private void SetAllArrowsFalse() {
        for (int i = 0; i < arrows.Length; i++) {
            arrows[i].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("EnemyProjectile")) {
            Damage(1);
        }
    }

    public void Damage(int damageAmt) {
        if (canBeDamaged) {
            playerHealth -= damageAmt;
            GetComponent<HealthBar>().ChangeHealth(playerHealth);
            if (damageFlash != null) {
                damageFlash.Flash();
            }
            currIFrameTime = 0f;
            if (playerHealth <= 0) {
                playerHealth = 0;
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die() {
		canMove = false;
        canChangeDir = false;
        movement = new Vector2(0, 0);
        dissolveShader.DissolveOut();
		GetComponent<Collider2D>().enabled = false;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(1.5f);
        ReferenceSingleton.Instance.transition.FadeOut();
        yield return new WaitForSeconds(1.5f);
        transform.position = spawn.transform.position;
		dissolveShader.DissolveIn();
        ReferenceSingleton.Instance.transition.FadeIn();
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(2f);
        canMove = true;
        canChangeDir = true;
        playerHealth = maxHealth;
		ChooseFacingDir();
        GetComponent<HealthBar>().ChangeHealth(playerHealth);
    }

    void OnMelee() {
        if (canMove) {
			AudioControl.Instance.PlaySFX("Melee Cast", gameObject, 0.2f, 0.5f);
			canMove = false;
			canChangeDir = false;
			animator.SetTrigger("doMelee");
            //ScreenShake.Instance.ShakeScreen(5f, .1f);
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
        animator.SetBool("isWalking", false);
    }


    public void ChangeSpawn(Transform newSpawn) {
        spawn = newSpawn;
		if (spawn.gameObject.tag == "DarkRoom") {
			isDark = true;
            light.SetActive(isDark);
		} else {
			isDark = false;
            light.SetActive(isDark);
		}

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
            canMove = false;
            transform.Translate(pushDir * pushSpd * Time.deltaTime);
            pushDist -= (pushDir *  pushSpd * Time.deltaTime).magnitude;
        }
    }

    public bool GetPushed() {
        return isPushed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("WindBlast")) {
            if (isPushed) {
                pushDist = 0;
                isPushed = false;
                canMove = true;
            }
        }
    }
}


