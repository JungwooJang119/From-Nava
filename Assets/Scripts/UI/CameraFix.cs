using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    [SerializeField] private Camera uiCam;
    private void Update() {

        uiCam.pixelRect = GetComponent<Camera>().pixelRect;
    }

}
