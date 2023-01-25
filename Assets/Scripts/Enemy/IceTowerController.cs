using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTowerController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject iceball;
    [SerializeField] private float iceballInterval;
    [SerializeField] private float currIceballTime;
    [SerializeField] private bool hasFired;

    public bool disableTower = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (disableTower) {
            Destroy(this.gameObject);
        }

        if (currIceballTime >= iceballInterval) {
            Instantiate(iceball, transform.position, Quaternion.identity);
            currIceballTime = 0;
        } else {
            currIceballTime += Time.deltaTime;
        }
    }
}
