using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMiniBossController : MonoBehaviour
{

    public float speed;
    public Transform player;
    public float minDistance;
    public float engageDistance;
    public float maxDistance;

    public float attackInterval;
    public float attackTime;
    public float currTime;

    public float changeTime;
    private float lastChangeTime;

    public bool attacking;

    public bool idle;

    private Enemy enemy;
    private EnemyState state;

    private GameObject weapon;

    private Vector2 pos1;
    private Vector2 pos2;

    private Vector2 oldPos;

    private Vector3 dir;

    private Animator animator;
    private string currentState;
    private Vector2 movement;
    private Vector2 direction;
    Transform t;
    public float fixedRotation = 0;

    public GameObject rightHB;
    public GameObject leftHB;
    public GameObject upHB;
    public GameObject downHB;


    //Animation States
    const string BIG_GUY_IDLE = "big_guy_idle_down";
    const string BIG_GUY_WALK = "big_guy_walk_down";
    const string BIG_GUY_ATTACK_RIGHT = "big_guy_attack_right";
    const string BIG_GUY_ATTACK_LEFT = "big_guy_attack_left";
    const string BIG_GUY_ATTACK_UP = "big_guy_attack_up";
    const string BIG_GUY_ATTACK_DOWN = "big_guy_attack_down";
    const string BIG_GUY_SPAWN_ENEMY = "big_guy_spawn_enemy";


    private void Start()
    {
        idle = true;
        attacking = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        enemy = GetComponent<Enemy>();
        enemy.ReactToPlayerInRange(!idle);
        weapon = transform.GetChild(0).gameObject;
        weapon.SetActive(false);
        NewDirection();
        currTime = 0;
        t = transform;
        animator = GetComponent<Animator>();
        state = GetComponent<Enemy>().currState;
        lastChangeTime = 0f;
    }

    private void NewDirection()
    {
        // find new direction
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movement = direction * speed;
    }

    void Update()
    {
        t.eulerAngles = new Vector3 (t.eulerAngles.x, fixedRotation, t.eulerAngles.z);
        currTime = Time.time - lastChangeTime;

        //print(state);

        switch(state) {
            case EnemyState.IDLE:
                if (enemy.GetPushed()) {
                    enemy.PushTranslate();
                }
                idle = true;
                enemy.ReactToPlayerInRange(false);
                ChangeAnimationState(BIG_GUY_WALK);
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
                idle = true;
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
                idle = false;
                ChangeAnimationState(BIG_GUY_WALK);
                if (Vector2.Distance(transform.position, player.position) > minDistance) {
                    enemy.ReactToPlayerInRange(true);
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                } else {
                    enemy.ReactToPlayerInRange(true);
                    if (!attacking && player.gameObject.GetComponent<PlayerController>().playerHealth > 0) {
                        state = EnemyState.ATTACK;
                    } else {
                        state = EnemyState.IDLE;
                    }
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
                
                if (!attacking && currTime >= attackInterval)
                {
                    //attacking = true;
                    if (Random.Range(1, 9) == 2) {
                        SpawnSmolGuy();
                    } else {
                        BigAttack();
                    }
                    currTime = 0;
                    //StartCoroutine(AnimationWait());
                }
                if (!attacking && Vector2.Distance(transform.position, player.position) > minDistance) {
                    state = EnemyState.CHASE;
                }
                if (!attacking && player.gameObject.GetComponent<PlayerController>().playerHealth <= 0) {
                    state = EnemyState.IDLE;
                }
                break;
        }
    }

    void BigAttack() {

        //Determine where to hit
        float px = player.position.x;
        float py = player.position.y;
        if (px > transform.position.x) {
            if (py > transform.position.y) {
                if ((px - transform.position.x) > (py - transform.position.y)) {
                    ChangeAnimationState(BIG_GUY_ATTACK_RIGHT);
                    return;
                    //rightHB.SetActive(true);
                } else {
                    ChangeAnimationState(BIG_GUY_ATTACK_UP);
                    return;
                    //upHB.SetActive(true);
                }
            } else {
                if ((px - transform.position.x) > (transform.position.y - py)) {
                    ChangeAnimationState(BIG_GUY_ATTACK_RIGHT);
                    return;
                    //rightHB.SetActive(true);
                } else {
                    ChangeAnimationState(BIG_GUY_ATTACK_DOWN);
                    return;
                    //downHB.SetActive(true);
                }
            }
        } else {
            if (py > transform.position.y) {
                if ((transform.position.x - px) > (py - transform.position.y)) {
                    ChangeAnimationState(BIG_GUY_ATTACK_LEFT);
                    return;
                    //leftHB.SetActive(true);
                } else {
                    ChangeAnimationState(BIG_GUY_ATTACK_UP);
                    return;
                    //upHB.SetActive(true);                
                } 
            } else {                        
                if ((transform.position.x - px) > (transform.position.y - py)) {
                    ChangeAnimationState(BIG_GUY_ATTACK_LEFT);
                    return;
                    //leftHB.SetActive(true);
                } else {
                    ChangeAnimationState(BIG_GUY_ATTACK_DOWN);
                    return;
                    //downHB.SetActive(true);
                    
                }
            }
        }
        //attacking = false;
    }

    void SpawnSmolGuy() {
        // animation: raises fist in air
        ChangeAnimationState(BIG_GUY_SPAWN_ENEMY);
    }

    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        NewDirection();
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

    IEnumerator AnimationWait() {
        yield return new WaitForSeconds(1.0f);
    }

        // // Update is called once per frame
    // void Update()
    // {
    //     if (transform.eulerAngles.z != 0)
    //     {
    //         transform.eulerAngles = new Vector3(0, 0, 0);
    //     }
        
    //     if (idle)
    //     {
    //         transform.position = Vector2.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed * 0.1f, 1.0f));
    //         // animator.SetFloat("X", .x);
    //         // animator.SetFloat("Y", movement.y);
    //         // animator.SetTrigger(doWalking);
    //     }
        
    //     if(!enemy.GetPushed()){
    //         if (Vector2.Distance(transform.position, player.position) < engageDistance || !idle)
    //         {
    //             idle = false;
    //             currTime += Time.deltaTime;
    //             if (Vector2.Distance(transform.position, player.position) > minDistance && !attacking)
    //             {
    //                 transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    //             }
    //             if (currTime >= attackInterval)
    //             {
    //                 dir = player.position - transform.position;
    //                 weapon.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    //                 attacking = true;
    //                 weapon.SetActive(true);
    //                 currTime = 0;
    //             }
    //             if (attacking && (currTime >= attackTime))
    //             {
    //                 attacking = false;
    //                 weapon.SetActive(false);
    //                 currTime = 0;
    //             }
    //         }
    //     } else {
    //         //enemy.PushTranslate();
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.gameObject == player.gameObject && collision.GetContact(0).collider.gameObject != this.gameObject)
    //     {
    //         print("player hit");
    //         //player.position = player.position + dir;
    //         player.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(2000*dir);
    //     }
    // }
}
