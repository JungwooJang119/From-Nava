using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = Random.Range(0, 1.0f);
        foreach (Transform child in transform) {
            child.GetComponent<LightVariance>().SetOffset(offset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
