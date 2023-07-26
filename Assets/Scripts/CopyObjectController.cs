using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyObjectController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private DealthDissolveShader dealthShader;

    public SpellScriptObj spell;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        dealthShader = GetComponent<DealthDissolveShader>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (health <= 0) {
            StartCoroutine(DeathSequence());
        }
        if (timer >= spell.lifetime) {
            StartCoroutine(DeathSequence());
        }
    }

    public void DestroyObject() {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence() {
        Destroy(this.GetComponent<BoxCollider2D>());
        dealthShader.DissolveOut();
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Spell") {
            health -= other.gameObject.GetComponent<Spell>().damage;
            if (damageFlash != null && other.gameObject.GetComponent<WindblastBehavior>() != true) {
                damageFlash.Flash();
            }
            return;
        }
        if (other.gameObject.tag == "Melee") {
            health -= other.gameObject.GetComponent<Melee>().damage;
            if (damageFlash != null && other.gameObject.GetComponent<WindblastBehavior>() != true) {
                damageFlash.Flash();
            }
            return;
        }
        if (other.gameObject.tag == "EnemyProjectile") {
            health -= other.gameObject.GetComponent<Projectile>().damage * 5;
            if (damageFlash != null && other.gameObject.GetComponent<WindblastBehavior>() != true) {
                damageFlash.Flash();
            }
            return;
        }
    }
}
