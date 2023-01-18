using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerAI : MonoBehaviour
{

    //public SpellScriptableObject SpellCast;
    public Spell SpellCast;
    public Transform castPoint;
    bool castingMagic = true;
    [SerializeField] private float currCastTimer;
    [SerializeField] private float magicCastInterval = 1f;
    public bool disableTower = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (castingMagic) {
            Instantiate(SpellCast, castPoint.position, Quaternion.identity);
            castingMagic = false;
            currCastTimer = 0;
        } else {
            currCastTimer += Time.deltaTime;
            if (currCastTimer > magicCastInterval) {
                castingMagic = true;
            }
        }
        if (disableTower) {
            Destroy(this.gameObject);
        }

    }
}
