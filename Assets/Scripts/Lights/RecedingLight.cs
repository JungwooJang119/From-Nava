using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RecedingLight : RoomLights {
    [SerializeField] private float propagationSpeed = 1f;
    [SerializeField] private RoomCode roomCode;
    private new Light2D light;
    private float intensity;
    private bool propagate;

    private void Awake() {
        light = GetComponent<Light2D>();
        intensity = light.intensity;
        light.intensity = 0;
        transform.parent.GetComponent<RoomLights>().OnPropagate += RoomLights_OnPropagate;
    }

    private void RoomLights_OnPropagate(RoomCode roomCode) {
        // Debug.Log("Got the propogation message! Checking aginst: " + this.roomCode);
        if (roomCode == this.roomCode) {
            propagate = true;
        }
    }

    private void Update() {
        if (propagate) {
            light.intensity = Approach(light.intensity, intensity, Time.deltaTime * propagationSpeed);
            if (light.intensity == intensity) {
                Destroy(this);
            } 
            
        }
    }

    // Approach one value to another;
    private float Approach(float currentValue, float targetValue, float rate) {
        rate = Mathf.Abs(rate);
        if (currentValue < targetValue) {
            currentValue += rate;
            if (currentValue > targetValue) return targetValue;
        } else {
            currentValue -= rate;
            if (currentValue < targetValue) return targetValue;
        } return currentValue;
    }
}



/// 
// Alternative Version
///

/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class RecedingLight : MonoBehaviour {

    [SerializeField] private float propagationSpeed = 1f;

    [Tooltip("Starting positions of the light. The order of the elements matters;")]
    [SerializeField] private int[] startCoordinates;

    [Tooltip("Vertex groups to be propagated by specific vertices. The internal and external order of vertices matters;")]
    [SerializeField] private VertexGroup[] vertexGroups;

    [SerializeField] private LightVertex[] vertices;
    [SerializeField] private List<LightVertex> activeVertices;
    private new Light2D light;
    private bool propagate;

    // Start is called before the first frame update
    void Start() {
        light = GetComponentInChildren<Light2D>();
        //light.intensity = 0;
        activeVertices = new List<LightVertex>();
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i].position = vertices[i].destination;
        }
    }

    // Update is called once per frame
    void Update() {
        var lightShape = new List<Vector3>();
        for (int i = 0; i < activeVertices.Count; i++) {
            var vertex = activeVertices[i];
            if (vertex.active) {
                activeVertices[i].position = new Vector3(Approach(vertex.position.x, vertex.destination.x, 
                                                         vertex.destination.x / transform.localPosition.x * propagationSpeed * Time.deltaTime),
                                                         Approach(vertex.position.y, vertex.destination.y,
                                                         vertex.destination.y / transform.localPosition.y * propagationSpeed * Time.deltaTime));
                if (vertex.position == vertex.destination) {
                    vertex.active = false;
                    if (vertex.propagateGroup > 0) { PropagateGroup(vertexGroups[vertex.propagateGroup - 1], vertex.destination); }
                }
            } lightShape.Add(activeVertices[i].position);
        } SetShapePath(lightShape.ToArray());
        if (Input.GetKeyUp("u") && !propagate) { Propagate(); propagate = true; }
    }

    public void Propagate() {
        foreach (int vertexIndex in startCoordinates) {
            var i = vertexIndex - 1;
            activeVertices.Add(vertices[i]);
        }
    }

    public void PropagateGroup(VertexGroup group, Vector3 startPosition) {
        for (int i = group.start - 1; i < group.end; i++) {
            vertices[i].position = startPosition;
            activeVertices.Add(vertices[i]);
        } activeVertices.Sort((v1, v2) => int.Parse(v1.name.Split(" ")[1]).CompareTo(int.Parse(v2.name.Split(" ")[1])));
    }

    public void PopulateLightController() {
        if (light == null) light = GetComponentInChildren<Light2D>();
        vertices = new LightVertex[light.shapePath.Length];
        for (int i = 0; i < light.shapePath.Length; i++) {
            vertices[i] = new LightVertex(light.shapePath[i]);
            vertices[i].name += (i + 1);
        }
    }

    void OnDrawGizmosSelected() {
        for (int i = 0; i < vertices.Length; i++) {
            var diskRadius = 0.25f;
            if (Camera.current != null) { diskRadius = 0.05f * Camera.current.orthographicSize; }
            Handles.DrawSolidDisc(Vector3.zero + transform.position + vertices[i].destination, Vector3.forward, diskRadius);

            var labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.red;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            Handles.Label(Vector3.zero + transform.position + vertices[i].destination, (i + 1).ToString(), labelStyle);
        }
    }

    public void ResetShape() {
        if (light == null) light = GetComponentInChildren<Light2D>();
        var vertexArray = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            vertexArray[i] = vertices[i].destination;
        } SetShapePath(vertexArray);
    }

    public void ClearLightController() {
        // To-Do;
    }

    // Set shapePath of freeform light
    private void SetShapePath(Vector3[] path) {
        var field = light.GetType().GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(light, path);
    }

    // Approach one value to another;
    private float Approach(float currentValue, float targetValue, float rate) {
        rate = Mathf.Abs(rate);
        if (currentValue < targetValue) {
            currentValue += rate;
            if (currentValue > targetValue) return targetValue;
        } else {
            currentValue -= rate;
            if (currentValue < targetValue) return targetValue;
        } return currentValue;
    }
}

[Serializable]
public class LightVertex {
    [HideInInspector] public string name = "Vertex ";
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 destination;
    public int propagateGroup;
    [HideInInspector] public bool active = true;

    public LightVertex(Vector3 destination) {
        this.destination = destination;
    }
}

[Serializable]
public class VertexGroup {
    [HideInInspector] public string name = "Group ";
    [Tooltip("Range of vertices belonging to the group;")]
    public int start; public int end;
    // Manipulated at runtime;
    private Vector3[] destinations;
}

[CustomEditor(typeof(RecedingLight))]
public class LightInspectorUtilEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        RecedingLight lightScript = (RecedingLight) target;

        if (GUILayout.Button("Generate Vertices")) {
            lightScript.PopulateLightController();
        } if (GUILayout.Button("Restore Shape")) {
            lightScript.ResetShape();
        } if (GUILayout.Button("Clear Vertices")) {
            lightScript.ClearLightController();
        }
    }
}
*/