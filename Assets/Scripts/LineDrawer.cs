using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 mousePos;
    private Vector2 startMousePos;
    public Material material;
    private int currLines = 0;

    [SerializeField] private TextMeshProUGUI distanceText;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        /*
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0)) {
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRend.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
            lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
            distance = (mousePos - startMousePos).magnitude;
            distanceText.text = distance.ToString("F2");
        } */

        if (Input.GetMouseButtonDown(0)) {
            if (line == null) {
                createLine();
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        } else if (Input.GetMouseButtonUp(0) && line) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(1, mousePos);
            line = null;
            currLines++;
        } else if (Input.GetMouseButton(0) && line) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.SetPosition(1, mousePos);
        }
    }

    void createLine() {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;
    }
}
