using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDesk : MonoBehaviour
{
    [SerializeField] private Sprite unlitScreen, litScreen;
    private LabReport labReport;
    private SpriteRenderer render;
    private bool hasRead;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        labReport = GetComponent<LabReport>();
        hasRead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasRead) {
            render.sprite = unlitScreen;
        }
        hasRead = labReport.GetUnlockStatus();
        
    }
}
