using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        AudioControl.Instance.PlaySFX("Explosion", gameObject, 0.7f);
        StartCoroutine(timer(time));
    }

    IEnumerator timer(float time) {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "CrackedWall") {
            Destroy(other.gameObject);
        }
    }
}
