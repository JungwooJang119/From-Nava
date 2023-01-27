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

    private GameObject range;
    private TrackPlayer detected;
    private bool yes;
    public bool disableTower = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        range = this.gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        detected = range.GetComponent<TrackPlayer>();
        yes = detected.GetPlayerIn();
        if (disableTower) {
            Destroy(this.gameObject);
        }
        if (detected) {
            if (currIceballTime >= iceballInterval) {
                Instantiate(iceball, transform.position, Quaternion.identity, gameObject.transform);
                currIceballTime = 0;
            } else {
                currIceballTime += Time.deltaTime;
            }
        }
    }
}
