using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    public int damage;

    private Vector3 playerPos;
    private LogicScript logic;
    private Vector2 dir; 

    // Start is called before the first frame update
    void Start()
    {
        playerPos = PlayerController.Instance.transform.position;
        dir = (new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y)).normalized;
        Destroy(this.gameObject, 4f); 
    }

    // Update is called once per frame
    // Use Collider not position
    void Update()
    {
        transform.Translate(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, transform.position.z);
    }
    
    //destroy the gameobject on collision with the player, make them take appropriate damage
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy")) {
            return;
        }
        if (col.gameObject.CompareTag("EnemyProjectile")) {
            return;
        }
        if (col.gameObject.CompareTag("IceTower")) {
            return;
        }
        if (col.gameObject.CompareTag("Chest")) {
            return;
        }
        if (col.gameObject.CompareTag("Player")) {
            /*
            if (hasParticles) {
    			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(this.gameObject, 0.5f);
                Instantiate(deathParticles, transform);
                
            }
            else {
                Destroy(this.gameObject);
            }*/
        }
        Destroy(this.gameObject);
    }
}
