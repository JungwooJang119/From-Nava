using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell_Behavior : MonoBehaviour
{
    [SerializeField] private float speedReduce;

    // Start is called before the first frame update
    void Start()
    {
        // Spell spell = GetComponent<Spell>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //reduces the speed on an enemy affected by ice
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            print("Speed reduction applied");
            other.gameObject.GetComponent<FireGoombaController>().speed /= speedReduce; // change this once the enemy has a speed variable
        }
    }
}
