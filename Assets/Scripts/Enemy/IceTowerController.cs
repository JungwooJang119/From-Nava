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

    [SerializeField] private float range;
    private bool detected;
    public bool disableTower = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currIceballTime = 0;
        //range = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.transform.position, player.position);
        if (dist < range) {
            detected = true;
        } else {
            detected = false;
        }
        if (disableTower) {
            Destroy(this.gameObject);
        }
        if (detected) {
            //Debug.Log(currIceballTime);
            if (currIceballTime >= iceballInterval) {
                Instantiate(iceball, transform.position, Quaternion.identity, gameObject.transform);
                currIceballTime = 0;
            } else {
                currIceballTime += Time.deltaTime;
            }
        }
    }
}
