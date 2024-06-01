using System.Collections;
using UnityEngine;

public class LightController : MonoBehaviour {

    [SerializeField] private float lightRate;
    private LightVariance[] lightVars;
    private float[] lightBounds;
    private float offset;

    void Awake() {
        offset = Random.Range(0, 1.0f);
        lightVars = GetComponentsInChildren<LightVariance>(true);
        lightBounds = new float[lightVars.Length];
        for (int i = 0; i < lightVars.Length; i++) {
            lightVars[i].Init(offset);
            lightBounds[i] = lightVars[i].BruhLight.pointLightOuterRadius;
        }
    }

    private IEnumerator MoveLightBoundsAsync(float speedMult, float boundWeight, float oscillation) {
        bool someBoundInProgress = true;
        while (someBoundInProgress) {
            someBoundInProgress = false;
            for (int i = 0; i < lightVars.Length; i++) {
                lightVars[i].AdjustRadius(Mathf.MoveTowards(lightVars[i].BaseRadius, lightBounds[i] * boundWeight + oscillation,
                                                            Time.deltaTime * lightRate * speedMult));
                if (!Mathf.Approximately(lightVars[i].BaseRadius, lightBounds[i] * boundWeight + oscillation)) someBoundInProgress = true;
            } yield return null;
        }
    }

    public void SetLightBounds(float boundWeight, float oscillation) {
        for (int i = 0; i < lightVars.Length; i++) {
            lightVars[i].AdjustRadius(lightBounds[i] * boundWeight + oscillation);
        }
    }

    public void MoveLightBounds(float speedMult, float boundWeight, float oscillation) {
        StopAllCoroutines();
        StartCoroutine(MoveLightBoundsAsync(speedMult, boundWeight, oscillation));
    }
}