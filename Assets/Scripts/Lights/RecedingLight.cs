using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;

public class RecedingLight : MonoBehaviour {

    [SerializeField] private float propagationSpeed = 1f;
    [SerializeField] private LightVertex[] vertices;
    private new Light2D light;
    private bool propagate;

    // Start is called before the first frame update
    void Start() {
        light = GetComponentInChildren<Light2D>();
        for (int i = 0; i < light.shapePath.Length; i++) {
            light.shapePath[i] = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update() {
        if (propagate) {
            for (int i = 0; i < light.shapePath.Length; i++) {
                var target = vertices[i].transform.localPosition;
                if (vertices[i].path.Count > 0) target = vertices[i].path[0].localPosition;
                light.shapePath[i] = new Vector2(Approach(light.shapePath[i].x, target.x, (target.x / transform.position.x) * propagationSpeed * Time.deltaTime),
                                                 Approach(light.shapePath[i].y, target.y, (target.y / transform.position.y) * propagationSpeed * Time.deltaTime));
                if (vertices[i].path != null && vertices[i].path.Count > 0 && light.shapePath[i].y == target.y) vertices[i].path.RemoveAt(0);
                Debug.Log(light.shapePath[i] + " vertex " + target + " speed " + (transform.position.y - target.y) * propagationSpeed);
            }
        }
        if (Input.GetKey("u")) propagate = true;
    }

    public void PopulateLightController() {
        if (light == null) light = GetComponentInChildren<Light2D>();
        vertices = new LightVertex[light.shapePath.Length];
        for (int i = 0; i < light.shapePath.Length; i++) {
            vertices[i] = new LightVertex();
            vertices[i].name += i + 1;
            var posGO = new GameObject("Light " + (i + 1));
            posGO.transform.SetParent(transform);
            posGO.transform.localPosition = light.shapePath[i];
            vertices[i].transform = posGO.transform;
        }
    }

    public void ClearLightController() {
        if (light == null) light = GetComponentInChildren<Light2D>();
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i].transform = transform;
        }
    }

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

[System.Serializable]
public class LightVertex {
    [HideInInspector] public string name = "Vertex ";
    [Tooltip("Position of the vertex in worldspace;")]
    public Transform transform;
    [Tooltip("Vertices to traverse before moving towards the final position. Use indexes;")]
    public List<Transform> path;
}

[CustomEditor(typeof(RecedingLight))]
public class LightInspectorUtilEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        RecedingLight lightScript = (RecedingLight) target;

        if (GUILayout.Button("Populate Coordinates")) {
            lightScript.PopulateLightController();
        } if (GUILayout.Button("Clear Coordinates")) {
            lightScript.ClearLightController();
        }
    }
}