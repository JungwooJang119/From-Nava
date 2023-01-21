using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerController : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField] private Transform castPoint;
    [SerializeField] private float currCastTimer;
    [SerializeField] private float castInterval = 1f;

    private bool castingMagic = true;
    
    public bool disableTower = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (castingMagic) {
            Instantiate(spell, castPoint.position, Quaternion.identity);
            castingMagic = false;
            currCastTimer = 0;
        } else {
            currCastTimer += Time.deltaTime;
            if (currCastTimer > castInterval) {
                castingMagic = true;
            }
        }
        if (disableTower) {
            Destroy(this.gameObject);
        }

    }
}
