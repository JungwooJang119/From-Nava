using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePlayerSettings 
{
    private float[] _volumeSettings;

    public SerializablePlayerSettings() {
        float[] dummy = {1.0f, 0.5f, 1.0f};
        _volumeSettings = dummy;
    }

    // Volume done via float[] array of Master, Music, Sfx
    public void SetVolumeSettings(float[] settings) {
        _volumeSettings = settings;
    }

    public float[] GetVolumeSettings() {
        return _volumeSettings;
    }

    public void PrintSettings() {
        Debug.Log("The current volume settings that we got are: " + _volumeSettings[0] + ", " + _volumeSettings[1] + ", " + _volumeSettings[2]);
    }
}
