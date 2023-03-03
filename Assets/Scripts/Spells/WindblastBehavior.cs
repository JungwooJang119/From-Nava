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
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().Push(spell.direction, pushDist, pushSpd);
        } else if (other.gameObject.CompareTag("Mirror")) {                                     //Mirror push adition, by Carlos.
            other.gameObject.GetComponent<Mirror>().Push(spell.direction, pushDist, pushSpd);   //Mirror script will mitigate push values.
        }
        if(other.gameObject.CompareTag("Fan")) {
            other.gameObject.GetComponent<Fan>().Blow();
        }
    }
}
