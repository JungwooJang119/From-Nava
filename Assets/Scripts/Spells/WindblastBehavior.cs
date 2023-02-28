using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindblastBehavior : MonoBehaviour
{
    [SerializeField] public float pushDist = 0f;
    private Spell spell;
    [SerializeField] public float pushSpd = 1f;
    // Start is called before the first frame update
    void Start()
    {
        spell = GetComponent<Spell>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().Push(spell.direction, pushDist, pushSpd);
        }
        if(other.gameObject.CompareTag("Fan")) {
            other.gameObject.GetComponent<Fan>().Blow();
        }
    }


}
