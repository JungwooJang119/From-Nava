using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGoombaControllerUpdated : MonoBehaviour
{

    public float speed;
    public Transform player;
    public float minDistance;
    public float maxDistance;

    public GameObject fireball;
    public float fireballInterval;
    public float currFireballTime;
    public bool hasFired;

    private Vector2 leftPoint;
    private Vector2 rightPoint;

    public bool idle;
    public float idleSpeed;

    private Vector2 direction;
    private Vector2 movement;

    public float changeTime;
    private float lastChangeTime;

    private void Start()
    {
        lastChangeTime = 0f;
        NewDirection();
    }

    private void NewDirection()
    {
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movement = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < maxDistance)
        {
            if (idle)
            {
                idle = false;
            }
            if (currFireballTime >= fireballInterval)
            {
                Instantiate(fireball, transform.position, Quaternion.identity);
                currFireballTime = 0;
            }
            else
            {
                currFireballTime += Time.deltaTime;
            }

            if (Vector2.Distance(transform.position, player.position) > minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (!idle)
            {
                idle = true;
                NewDirection();
            }
            if (Time.time - lastChangeTime > changeTime)
            {
                lastChangeTime = Time.time;
                NewDirection();
            }
            transform.position = new Vector2(transform.position.x + (movement.x * Time.deltaTime), transform.position.y + (movement.y * Time.deltaTime));
        }
    }
}
