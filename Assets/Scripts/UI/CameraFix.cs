using UnityEngine;

public class CameraFix : MonoBehaviour {
    [SerializeField] private Camera uiCam;
    private Camera cam;
    void Start() => cam = GetComponent<Camera>();
    void Update() => uiCam.pixelRect = cam.pixelRect;
}
