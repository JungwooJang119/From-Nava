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

public class Enemy : MonoBehaviour, IDamageable, IPushable
{
    public event Action<int> OnDamageTaken;
    public event Action<Enemy, bool> OnPlayerInRange;
    public event Action OnDeath;
    [SerializeField] private int maxHealth = 50;

    [SerializeField] private GameObject healthBar;
    public bool isIceTower = false;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private DealthDissolveShader dealthShader;
    [SerializeField] private GameObject tutorialSpellObject;
    private int currHealth;

    private bool isPushed;
    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;

    public UnityEvent onMeleeHit;

    private Animator animator;

    private SpriteRenderer sr;

    public EnemyState currState;

    private void Start() {
        OnPlayerInRange += BattleManager.Instance.RegisterEnemy;

        currHealth = maxHealth;
        isPushed = false;
        animator = GetComponent<Animator>();
        dealthShader = GetComponent<DealthDissolveShader>();
        sr = GetComponent<SpriteRenderer>();

        healthBar.GetComponent<EnemyHealthBar>().SetUp((int) maxHealth, (int) currHealth);
    }

    void OnDestroy() {
        OnPlayerInRange?.Invoke(this, false);
        OnDeath?.Invoke();
    }

    public void Damage(int damage) {

        currHealth -= damage;
        OnDamageTaken?.Invoke((int) damage);
        if (damageFlash != null)
        {
            damageFlash.Flash();
        }
        if (currHealth <= 0) {
            if (!isIceTower) {
                animator.SetBool("isDead", true);
            }
            StartCoroutine(DeathSequence());
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

    IEnumerator DeathSequence() {
        dealthShader.DissolveOut();
        yield return new WaitForSeconds(1f);
        if (tutorialSpellObject) {
            tutorialSpellObject.transform.position = gameObject.transform.position;
        }
        Destroy(this.gameObject);
    }

    public bool CheckIsAlive() {
        return currHealth > 0;
    }

    public void ReactToPlayerInRange(bool playerInRange) {
        healthBar.SetActive(true);

        OnPlayerInRange?.Invoke(this, playerInRange);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("WindBlast")) {
            if (isPushed) {
                pushDist = 0;
                isPushed = false;
            }
        }
    }
}
