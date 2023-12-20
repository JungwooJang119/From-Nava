using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private bool rotating;
    [SerializeField] private float windIncrement = 5f;
    private float rotateTimer;
    // private Color whenRotate = Color.blue;
    // private Color notRotate = Color.red;
    SpriteRenderer sr;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rotating = false;
        rotateTimer = 0;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // sr.color = rotating ? whenRotate : notRotate;
        if (rotateTimer > 0) {
            rotating = true;
            rotateTimer -= Time.deltaTime;
            animator.SetBool("IsSpinning", true);
        } else {
            rotating = false;
            animator.SetBool("IsSpinning", false);
        }
    }

    public void Blow() {
        rotateTimer = windIncrement;
    }

    public bool IsBlowing() {
        return rotating;
    }
}
