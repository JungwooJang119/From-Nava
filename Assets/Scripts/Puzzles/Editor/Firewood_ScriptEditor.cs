using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Firewood))]
public class Firewood_ScriptEditor : Editor {

    Firewood FirewoodScript => target as Firewood;
    private bool editorTools;
    private float searchRange = 4f;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        editorTools = EditorGUILayout.BeginFoldoutHeaderGroup(editorTools, "Editor Tools");
        if (editorTools) {
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Assign Firewoods", GUILayout.MaxWidth(176))) {
                AssignFirewoods(searchRange);
            } EditorGUILayout.Space();
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(3);
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Search Range: ", GUILayout.MaxWidth(96));
            searchRange = EditorGUILayout.FloatField(searchRange, GUILayout.MaxWidth(48));
            EditorGUILayout.Space();
            GUILayout.EndHorizontal();
        } EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void AssignFirewoods(float range) {
        var colliders = new List<Collider2D>(FirewoodScript.GetComponentsInChildren<Collider2D>());
        var hits = new List<RaycastHit2D>();
        Physics2D.CircleCast(FirewoodScript.transform.position, range, Vector2.zero, new ContactFilter2D().NoFilter(), hits);
        var validGOs = new List<GameObject>();
        foreach (RaycastHit2D hitData in hits) {
            if (!colliders.Contains(hitData.collider) && hitData.collider.isTrigger) {
                validGOs.Add(hitData.transform.gameObject);
            }
        } FirewoodScript.EDITOR_AssignFirewoods(validGOs.ToArray());
    }
}