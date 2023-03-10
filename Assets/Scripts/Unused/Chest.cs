using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PanCamera pcam;
    private bool isActive = true;

    void Start()
    {
        pcam = GameObject.FindGameObjectWithTag("PanCamera").GetComponent<PanCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            pcam.startPan();
            isActive = false;
        }
    }
}
