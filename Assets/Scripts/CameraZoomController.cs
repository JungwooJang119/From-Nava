using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraZoomController : MonoBehaviour
{
    private float standardZoom;
    private CinemachineVirtualCamera cam;
    private PixelPerfectCamera ppc;

    void Awake() {
        ppc = GetComponent<PixelPerfectCamera>();
        cam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        standardZoom = cam.m_Lens.OrthographicSize;
    }

    public void BeginZoom(float zoom, float zoomSpeed) {
        ppc.enabled = false;
        StartCoroutine(Zoom(zoom, zoomSpeed));
    }

    public void RestoreZoom(float zoomSpeed) {
        StopAllCoroutines();
        StartCoroutine(Zoom(standardZoom, zoomSpeed, true));
    }

    private IEnumerator Zoom(float zoom, float zoomSpeed, bool enablePPC = false) {
        while (cam.m_Lens.OrthographicSize != zoom) {
            cam.m_Lens.OrthographicSize = Mathf.MoveTowards(cam.m_Lens.OrthographicSize, zoom, zoomSpeed);
            yield return null;
        } if (enablePPC) ppc.enabled = true;
    }
}
