using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    [SerializeField] private bool hasParticles;


    private Vector3 playerPos;
    private LogicScript logic;
    private Vector2 dir; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        logic = GameObject.Find("Logic Manager").GetComponent<LogicScript>();
        dir = (new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y)).normalized;
    }

    // Update is called once per frame
    // Use Collider not position
    void Update()
    {
        transform.Translate(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, transform.position.z);
    }
    
    //destroy the gameobject on collision with the player, make them take appropriate damage
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player")) {
            logic.TakeDamage(damage);
            if(hasParticles) {
    			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(this.gameObject, 0.5f);
                Instantiate(deathParticles, transform);
            }
            else {
                Destroy(this.gameObject);

            }
        }
    }
}
