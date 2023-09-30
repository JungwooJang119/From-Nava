using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set;}
    private CinemachineVirtualCamera camera;
    private float timer;
    private float durationTimer;
    private float startingIntensity;
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeScreen(float intensity, float timeDuration) {
        CinemachineBasicMultiChannelPerlin cbmp = 
            camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmp.m_AmplitudeGain = intensity;
        timer = timeDuration;
        startingIntensity = intensity;
        durationTimer = timeDuration;
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cbmp = 
               camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (timer / durationTimer));
        }
        // Less smooth version
        // if (timer > 0) {
        //     timer -= Time.deltaTime;
        //     if (timer <= 0f) {
        //         CinemachineBasicMultiChannelPerlin cbmp = 
        //             camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //         cbmp.m_AmplitudeGain = 0f;
        //     }
        // }
        //
    }
}
