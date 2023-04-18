using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMiniBossController : MonoBehaviour
{

    public float speed;
    public Transform player;
    public float minDistance;
    public float engageDistance;

    public float attackInterval;
    public float attackTime;
    public float currTime;

    private bool attacking;

    public bool idle;

    private Enemy enemy;

    private GameObject weapon;

    private Vector2 pos1;
    private Vector2 pos2;

    private Vector3 dir;


    private void Start()
    {
        idle = true;
        attacking = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        enemy = GetComponent<Enemy>();
        weapon = transform.GetChild(0).gameObject;
        weapon.SetActive(false);
        currTime = 0;
        pos1 = transform.position + new Vector3(2, 0, 0);
        pos2 = transform.position + new Vector3(-2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.z != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
        if (idle)
        {
            transform.position = Vector2.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed * 0.1f, 1.0f));
        }
        
        if(!enemy.GetPushed()){
            if (Vector2.Distance(transform.position, player.position) < engageDistance || !idle)
            {
                idle = false;
                currTime += Time.deltaTime;
                if (Vector2.Distance(transform.position, player.position) > minDistance && !attacking)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
                if (currTime >= attackInterval)
                {
                    dir = player.position - transform.position;
                    weapon.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
                    attacking = true;
                    weapon.SetActive(true);
                    currTime = 0;
                }
                if (attacking && (currTime >= attackTime))
                {
                    attacking = false;
                    weapon.SetActive(false);
                    currTime = 0;
                }
            }
        } else {
            //enemy.PushTranslate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player.gameObject && collision.GetContact(0).collider.gameObject != this.gameObject)
        {
            print("player hit");
            //player.position = player.position + dir;
            player.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(2000*dir);
        }
    }
}
