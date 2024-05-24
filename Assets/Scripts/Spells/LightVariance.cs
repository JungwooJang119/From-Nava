using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightVariance : MonoBehaviour {

    [SerializeField] private float baseIntensity;
    [SerializeField] private float radiusOffset;

    private Light2D bruhLight;
    public Light2D BruhLight {
        get {
            if (bruhLight == null) bruhLight = GetComponent<Light2D>();
            return bruhLight;
        }
    } public float BaseRadius { get; private set; }
    private float currOffset;
    private float timeOffset;
    private float intensityOffset;

    void Awake() => bruhLight = GetComponent<Light2D>();
    void OnEnable() => StartCoroutine(OffsetRandomizer());
    private void OnDisable() => StopAllCoroutines();

    public void Init(float o) => intensityOffset = o;

    void Update() {
        currOffset = Mathf.MoveTowards(currOffset, Mathf.PingPong(Time.time * timeOffset, radiusOffset), Time.deltaTime / 4f);
        bruhLight.pointLightOuterRadius = currOffset + BaseRadius;
        bruhLight.intensity = Mathf.PingPong(intensityOffset + Time.time / 4, 0.1f) + baseIntensity;
    }

    private IEnumerator OffsetRandomizer() {
        while (true) {
            timeOffset = Random.Range(0.8f, 1.2f);
            yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        }
    }

    public void AdjustRadius(float radius) => BaseRadius = radius;
}
