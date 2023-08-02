using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyState {
    IDLE,
    WANDER,
    CHASE,
    ATTACK,
    PUSHED,
    STUNNED,
    SLOWED,
    DEAD
}

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<int> OnDamageTaken;
    public event Action<bool> OnPlayerInRange;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private GameObject healthBar;
    public bool isIceTower = false;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private DealthDissolveShader dealthShader;
    private float currHealth;

    private bool isPushed;
    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;

    public UnityEvent onMeleeHit;

    private Animator animator;

    private SpriteRenderer sr;

    public EnemyState currState;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        currHealth = maxHealth;
        isPushed = false;
        animator = GetComponent<Animator>();
        dealthShader = GetComponent<DealthDissolveShader>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        healthBar.GetComponent<EnemyHealthBar>().SetUp((int) maxHealth, (int) currHealth);
    }

    public void TakeDamage(int dmgAmount, GameObject srcObject) {
        currHealth -= dmgAmount;
        OnDamageTaken?.Invoke((int) dmgAmount);
        if (damageFlash != null)
        {
            damageFlash.Flash();
        }
        //Debug.Log(currHealth);
        if (currHealth <= 0) {
            dealthShader.DissolveOut();
            if (!isIceTower) {
                animator.SetBool("isDead", true);
                StartCoroutine(DeathSequence());
            } else {
                Destroy(this.gameObject, 1f);
            }
        }
    }

    public void Push(Vector2 dir, float dist, float spd) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
    }

    public void PushTranslate() {
        if (pushDist <= 0) {
            isPushed = false;
        } else {
            transform.Translate(pushDir * pushSpd * Time.deltaTime);
            pushDist -= (pushDir *  pushSpd * Time.deltaTime).magnitude;
        }
    }

    public bool GetPushed() {
        return isPushed;
    }

    // void OnMeleeHit(float meleeDamage) {
    //     if (isIceTower) {
    //         return;
    //     }
    //     TakeDamage(meleeDamage);
    //     onMeleeHit?.Invoke();
        
    // }

    IEnumerator DeathSequence() {
        dealthShader.DissolveOut();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public void ReactToPlayerInRange(bool playerInRange) {
        healthBar.SetActive(true);
        OnPlayerInRange?.Invoke(playerInRange);
    }
}
