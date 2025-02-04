using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{
    private float[] _volumeSettings;

    
    // Volume done via float[] array of Master, Music, Sfx
    public void SetVolumeSettings(float[] settings) {
        _volumeSettings = settings;
    }

    public float[] GetVolumeSettings() {
        return _volumeSettings;
    }

    public void PrintSettings() {
        Debug.Log("The current volume settings that we got are: " + _volumeSettings);
    }

    
}
