using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomLights : MonoBehaviour {
    public enum RoomCode {
        None,
        A1_2,
        A2_2,
        A2_3,
        B3_2,
        B3_3,
        C2_2,
        C2_3
    }

    public event Action<RoomCode> OnPropagate;

    public void Propagate(RoomCode roomCode) {
        OnPropagate?.Invoke(roomCode);
    }

    public void SetLightIntensity(float intensity) {
        foreach (Transform t in transform) {
            t.GetComponent<Light2D>().intensity = intensity;
        }
    }
}
