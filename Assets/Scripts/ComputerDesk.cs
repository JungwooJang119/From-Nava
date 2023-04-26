using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDesk : MonoBehaviour
{   
    [SerializeField] private Sprite unlitScreen, litScreen;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Start() {
        GetComponent<LabReport>().OnReportRead += ComputerDesk_OnReportRead;
		render = GetComponent<SpriteRenderer>();
    }

    private void ComputerDesk_OnReportRead() {
		render.sprite = unlitScreen;
	}
}
