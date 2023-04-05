using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private bool isIceTower;
    private float currHealth;

    private bool isPushed;
    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;

    public UnityEvent onMeleeHit;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        currHealth = maxHealth;
        isPushed = false;
        isIceTower = false;
    }

    public void TakeDamage(float damage) {
        currHealth -= damage;
        //Debug.Log(currHealth);
        if (currHealth <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void Push(Vector2 dir, float dist, float spd) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
    }

    public void PushTranslate() {
        Debug.Log(pushDist);
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
}
