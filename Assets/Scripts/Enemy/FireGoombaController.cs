using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGoombaController : MonoBehaviour
{

    public float speed;
    public Transform player;
    public GameObject spawn;//added for fake chest
    public float minDistance;
    public float maxDistance;
    private Animator animator;

    public GameObject fireball;
    public float fireballInterval;
    public float currFireballTime;
    public bool hasFired;

    public bool idle;

    private Vector2 direction;
    private Vector2 movement;

    public float changeTime;
    private float lastChangeTime;

    private Enemy enemy;


    Transform t;
    public float fixedRotation = 0;



    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemy.ReactToPlayerInRange(!idle);
        animator = GetComponent<Animator>();
        lastChangeTime = 0f;
        NewDirection();
        t = transform;
        //added for fake chest
        spawn = GameObject.FindWithTag("Player");
        player = spawn.transform;
    }

    private void NewDirection()
    {
        // idle, no attack
        animator.SetBool("Attack", false);
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movement = direction * speed;
    }

    void Update()
    {
        t.eulerAngles = new Vector3 (t.eulerAngles.x, fixedRotation, t.eulerAngles.z);
        if(!enemy.GetPushed()){
            if (Vector2.Distance(transform.position, player.position) < maxDistance) {
                if (idle) {
                    idle = false;
                    enemy.ReactToPlayerInRange(!idle);
                }
                if (currFireballTime >= fireballInterval) {
                    currFireballTime = 0;
                    StartCoroutine(Attack());
                } else {
                    currFireballTime += Time.deltaTime;
                }
                // chase mode
                if (Vector2.Distance(transform.position, player.position) > minDistance) {
                    animator.SetBool("Attack", true);
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
            } else {
                if (!idle) {
                    idle = true;
                    enemy.ReactToPlayerInRange(!idle);
                    NewDirection();
                }
                if (Time.time - lastChangeTime > changeTime) {
                    lastChangeTime = Time.time;
                    NewDirection();
                }
                transform.position = new Vector2(transform.position.x + (movement.x * Time.deltaTime), transform.position.y + (movement.y * Time.deltaTime));
            }
        } else {
            enemy.PushTranslate();
        }
    }

    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        NewDirection();
    }

    IEnumerator Attack() {
        animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(0.3f);
        Instantiate(fireball, transform.position, Quaternion.identity);
        animator.SetBool("isFiring", false);
    }
}
