using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PauseMenuBookmark))]
public class PauseMenuBookmarkEditor : Editor {

    SerializedProperty colorNormal;
    SerializedProperty colorHighlight;
    SerializedProperty colorPressed;
    SerializedProperty colorSelected;

    private void OnEnable() {
        colorNormal = serializedObject.FindProperty("colorNormal");
        colorHighlight = serializedObject.FindProperty("colorHighlight");
        colorPressed = serializedObject.FindProperty("colorPressed");
        colorSelected = serializedObject.FindProperty("colorSelected");
    }

    private bool showColors;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        serializedObject.Update();
        showColors = EditorGUILayout.Foldout(showColors, "Button Colors");
        if (showColors) {
            EditorGUILayout.PropertyField(colorNormal);
            EditorGUILayout.PropertyField(colorHighlight);
            EditorGUILayout.PropertyField(colorPressed);
            EditorGUILayout.PropertyField(colorSelected);
        }
        serializedObject.ApplyModifiedProperties();
    }
}