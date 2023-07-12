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
    public float spacedDistance;
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
    public float currTime;

    private Enemy enemy;

    private EnemyState state;


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
        state = GetComponent<Enemy>().currState;
    }

    private void NewDirection()
    {
        // find new direction
        animator.SetBool("Attack", false);
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movement = direction * speed;
    }

    void Update()
    {
        t.eulerAngles = new Vector3 (t.eulerAngles.x, fixedRotation, t.eulerAngles.z);
        currTime = Time.time - lastChangeTime;

        switch(state) {
            case EnemyState.IDLE:
                if (enemy.GetPushed()) {
                    enemy.PushTranslate();
                }
                enemy.ReactToPlayerInRange(false);
                //NewDirection();
                if (Time.time - lastChangeTime > changeTime) {
                    lastChangeTime = Time.time;
                    NewDirection();
                }
                transform.position = new Vector2(transform.position.x + (movement.x * Time.deltaTime), transform.position.y + (movement.y * Time.deltaTime));
                if (Vector2.Distance(transform.position, player.position) < minDistance) {
                    state = EnemyState.CHASE;
                }
                break;
            case EnemyState.WANDER:
                if (enemy.GetPushed()) {
                    enemy.PushTranslate();
                }
                enemy.ReactToPlayerInRange(false);
                //NewDirection();
                if (Time.time - lastChangeTime > changeTime) {
                    lastChangeTime = Time.time;
                    NewDirection();
                }
                transform.position = new Vector2(transform.position.x + (movement.x * Time.deltaTime), transform.position.y + (movement.y * Time.deltaTime));
                if (Vector2.Distance(transform.position, player.position) < minDistance) {
                    state = EnemyState.CHASE;
                }
                break;
            case EnemyState.CHASE:
                if (enemy.GetPushed()) {
                    enemy.PushTranslate();
                }
                if (Vector2.Distance(transform.position, player.position) > minDistance) {
                    enemy.ReactToPlayerInRange(true);
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                } else {
                    enemy.ReactToPlayerInRange(true);
                    state = EnemyState.ATTACK;
                }
                if (Vector2.Distance(transform.position, player.position) > maxDistance) {
                    state = EnemyState.IDLE;
                }
                break;
            case EnemyState.ATTACK:
                if (enemy.GetPushed()) {
                    enemy.PushTranslate();
                }
                enemy.ReactToPlayerInRange(true);
                if (currFireballTime >= fireballInterval) {
                    currFireballTime = 0;
                    StartCoroutine(Attack());
                } else {
                    currFireballTime += Time.deltaTime;
                }
                if (Vector2.Distance(transform.position, player.position) > minDistance) {
                    currFireballTime = 0;
                    state = EnemyState.CHASE;
                }
                break;

        }
    }

    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        NewDirection();
    }

    IEnumerator Attack() {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        Instantiate(fireball, transform.position, Quaternion.identity);
        animator.SetBool("Attack", false);
    }

}
