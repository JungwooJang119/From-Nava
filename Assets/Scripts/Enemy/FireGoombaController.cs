using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGoombaController : MonoBehaviour
{

    public float speed;
    public Transform player;
    public float minDistance;

    public GameObject fireball;
    public float fireballInterval;
    public float currFireballTime;
    public bool hasFired;

    // Update is called once per frame
    void Update()
    {
        if (currFireballTime >= fireballInterval) {
            Instantiate(fireball, transform.position, Quaternion.identity);
            currFireballTime = 0;
        } else {
            currFireballTime += Time.deltaTime;
        }

        if(Vector2.Distance(transform.position, player.position) > minDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
