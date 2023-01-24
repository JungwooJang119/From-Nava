using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;

    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    // Use Collider not position
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        if (transform.position == playerPos) {
            Destroy(gameObject);
        }
    }
}
