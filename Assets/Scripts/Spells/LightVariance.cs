using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVariance : MonoBehaviour
{
    [SerializeField] float baseLight;
    private UnityEngine.Rendering.Universal.Light2D bruhLight;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {

        bruhLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    public void SetOffset(float o) {
        offset = o;
    }
    // Update is called once per frame
    void Update()
    {
        bruhLight.intensity = Mathf.PingPong(offset + Time.time / 4, 0.1f) + baseLight;
    }
}
