using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVariance : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {

        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    public void SetOffset(float o) {
        offset = o;
    }
    // Update is called once per frame
    void Update()
    {
        light.intensity = Mathf.PingPong(offset + Time.time / 4, 0.1f) + 0.35f;
    }
}
