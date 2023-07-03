using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomLights))]
public class RoomLightsEditor : Editor {
    float intensityValue = 1f;
    public override void OnInspectorGUI() {

        var script = (RoomLights) target;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        intensityValue = EditorGUILayout.FloatField(intensityValue, GUILayout.Width(50));
        if (GUILayout.Button("Set Light Intensity", GUILayout.Width(200))) {
            script.SetLightIntensity(intensityValue);
        } EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }
}
