using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    public bool isIceTower = false;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private DealthDissolveShader dealthShader;
    [SerializeField] private Material defaultLit;
    [SerializeField] private Material dissolve;
    [SerializeField] private bool isInDarkRoom;
    private float currHealth;

    private bool isPushed;
    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;

    public UnityEvent onMeleeHit;

    private Animator animator;

    private SpriteRenderer sr;

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
        if (isInDarkRoom) {
            sr.material = defaultLit;
        } else {
            sr.material = dissolve;
        }
    }

    public void TakeDamage(float damage) {
        currHealth -= damage;
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

    void OnMeleeHit(float meleeDamage) {
        if (isIceTower) {
            return;
        }
        TakeDamage(meleeDamage);
        onMeleeHit?.Invoke();
        
    }

    IEnumerator DeathSequence() {
        dealthShader.DissolveOut();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Melee") {
            OnMeleeHit(10);
        }
    }
}
