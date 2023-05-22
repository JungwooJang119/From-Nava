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
    private Enemy enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currIceballTime = 0;
        enemy.ReactToPlayerInRange(detected);
        //range = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.transform.position, player.position);
        if (dist < range) {
            if (!detected) enemy.ReactToPlayerInRange(!detected);
            detected = true;
        } else {
            if (detected) enemy.ReactToPlayerInRange(!detected);
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
