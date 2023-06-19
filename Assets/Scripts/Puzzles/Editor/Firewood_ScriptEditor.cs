using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Firewood_Script))]
public class Firewood_ScriptEditor : Editor {

    bool editorTools;
    float searchRange = 4f;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Firewood_Script firewoodScript = (Firewood_Script) target;

        editorTools = EditorGUILayout.BeginFoldoutHeaderGroup(editorTools, "Editor Tools");
        if (editorTools) {
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Assign Firewoods", GUILayout.MaxWidth(176))) {
                firewoodScript.AssignFirewoods(searchRange);
            }
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(3);
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Search Range: ", GUILayout.MaxWidth(96));
            searchRange = EditorGUILayout.FloatField(searchRange, GUILayout.MaxWidth(48));
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}