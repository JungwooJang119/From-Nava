using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float strength = 16f, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;

    public void PlayKnockback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    // Set velocity to zero after delay
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();
    }

}
