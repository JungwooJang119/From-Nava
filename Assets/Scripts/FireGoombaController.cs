using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGoombaController : MonoBehaviour
{

    public float speed;
    public Transform target;
    public float minDistance;

    public GameObject fireball;
    public float fireballInterval;
    public float currFireballTime;
    public bool hasFired;

    // Update is called once per frame
    void Update()
    {

        if (Time.time > currFireballTime) {
            Instantiate(fireball, transform.position, Quaternion.identity);
            currFireballTime = Time.time + fireballInterval;
        }


        if(Vector2.Distance(transform.position, target.position) > minDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } /*else {
            if (hasFired) {
                currFireballTime += Time.deltaTime;
                if (currFireballTime >= fireballInterval) {
                    hasFired = false;
                }
            } else {
                Instantiate(fireball, transform.position, Quaternion.identity);
                hasFired = true;
                currFireballTime =
            } */
    }
}
