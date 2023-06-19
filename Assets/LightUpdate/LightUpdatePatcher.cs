using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomLights;
using UnityEditor;

[ExecuteInEditMode]
public class LightUpdatePatcher : MonoBehaviour {
    public void PatchGame() {
        // Disable global light;
        GameObject.Find("Light 2D").gameObject.SetActive(false);
        // Generate Freeform Lights
        PrefabUtility.InstantiatePrefab(Resources.Load("RoomLights"));
        PrefabUtility.UnpackPrefabInstance(GameObject.Find("RoomLights"), PrefabUnpackMode.Completely, InteractionMode.UserAction);
        // Update RoomControl variables
        var roomControl = GameObject.Find("RoomControl").transform;
        var tempComponent1 = QueueFind(roomControl, "A2 - 1").GetComponent<RoomControlA2>();
        tempComponent1.SetRoomCode(RoomCode.A2_2);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempComponent1);
        tempComponent1 = QueueFind(roomControl, "A2 - 2").GetComponent<RoomControlA2>();
        tempComponent1.SetRoomCode(RoomCode.A2_3);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempComponent1);
        var tempComponent2 = QueueFind(roomControl, "B3 - 1").GetComponent<RoomControlA3>();
        tempComponent2.cameraTarget = tempComponent2.door;
        tempComponent2.SetRoomCode(RoomCode.B3_2);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempComponent2);
        tempComponent2 = QueueFind(roomControl, "B3 - 2").GetComponent<RoomControlA3>();
        tempComponent2.SetRoomCode(RoomCode.B3_3);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempComponent2);
        // Update Terminal variables
        var tempTerminal = GameObject.Find("C2 - 1").transform.Find("Terminal");
        tempTerminal.GetComponent<LaserTerminal>().SetRoomCode(RoomCode.C2_2);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempTerminal.GetComponent<LaserTerminal>());
        tempTerminal = GameObject.Find("C2 - 2").transform.Find("Terminal");
        tempTerminal.GetComponent<LaserTerminal>().SetRoomCode(RoomCode.C2_3);
        PrefabUtility.RecordPrefabInstancePropertyModifications(tempTerminal.GetComponent<LaserTerminal>());
        EditorUtility.DisplayDialog("Patch Complete", "Scene Patch Successful! (Probably)\nYou can now delete the LightUpdate Folder under Assets. We don't need it anymore ;-;", ":happypiplup:");
        StartCoroutine(WaitForRoomLights());
    }

    // Queues are AMAZING OMG!
    public Transform QueueFind(Transform transform, string name) {
        Queue<Transform> tQueue = new Queue<Transform>();
        tQueue.Enqueue(transform);
        while (tQueue.Count > 0) {
            var t = tQueue.Dequeue();
            if (t.gameObject.name == name) return t;
            foreach (Transform child in t) tQueue.Enqueue(child);
        } return null;
    }

    private IEnumerator WaitForRoomLights() {
        while (!GameObject.Find("RoomLights").GetComponent<RoomLights>()) yield return null;
        GameObject.Find("ReferenceSingleton").GetComponent<ReferenceSingleton>().roomLights = GameObject.Find("RoomLights").GetComponent<RoomLights>();
        PrefabUtility.RecordPrefabInstancePropertyModifications(GameObject.Find("ReferenceSingleton").GetComponent<ReferenceSingleton>());
        DestroyImmediate(gameObject);
    }
}

[CustomEditor(typeof(LightUpdatePatcher))]
public class LightUpdatePatcherEditor : Editor {

    public override void OnInspectorGUI() {
        LightUpdatePatcher patchScript = (LightUpdatePatcher) target;

        EditorGUILayout.LabelField("Press This Button To Patch the M3 Scene. I wrote this line by line so...");
        EditorGUILayout.LabelField("Totally safe yk (probably, yes >.>)");
        if (GUILayout.Button("Patch Scene")) {
            patchScript.PatchGame();
        }
    }
}
