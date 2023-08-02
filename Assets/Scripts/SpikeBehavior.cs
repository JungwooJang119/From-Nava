using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    [SerializeField] private int dmg = 1;
    [SerializeField] private float knockbackSpd = 3f;
    [SerializeField] private float knockbackDist = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {        
        if (other.gameObject.CompareTag("Enemy")) { // and enemy is in pushed state
            Vector2 avgCollision = new Vector2(0, 0);

            for (int i = 0; i < other.contactCount; i++) {
                avgCollision += other.GetContact(i).point;
            }
            avgCollision /= other.contactCount;

            Vector3 contact = new Vector3(avgCollision.x, avgCollision.y, other.transform.position.z);
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector3 pushDir = Vector3.Normalize(other.transform.position - contact);
            other.gameObject.GetComponent<Enemy>().Push(pushDir, knockbackDist, knockbackSpd);
            //other.gameObject.GetComponent<Enemy>().TakeDamage((float)dmg);
        } else if (other.gameObject.CompareTag("Player")) {
            Vector2 avgCollision = new Vector2(0, 0);

            for (int i = 0; i < other.contactCount; i++) {
                avgCollision += other.GetContact(i).point;
            }
            avgCollision /= other.contactCount;

            Vector3 contact = new Vector3(avgCollision.x, avgCollision.y, other.transform.position.z);
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector3 pushDir = Vector3.Normalize(other.transform.position - contact);
            Debug.Log(pushDir);

            //other.gameObject.GetComponent<PlayerController>().Push(pushDir, knockbackDist, knockbackSpd); // if knockbackDist is set to 0, this must be commented or else player
            // canMove will default to true even when dying in couroutine Die()
            other.gameObject.GetComponent<PlayerController>().TakeDamage(dmg, other.gameObject);

        }
    }
}
