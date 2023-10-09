using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPolaroid : MonoBehaviour {

    public event System.Action OnScriptEnd;
    
    [SerializeField] private tranMode transition;
    [SerializeField] private EndTutorial tut;
    [SerializeField] private EndPolaroidDisplay controller;

    void Awake() {
        tut = GetComponentInChildren<EndTutorial>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            controller.gameObject.SetActive(true);
            tut.Fade();
        } if (controller == null) {
            OnScriptEnd?.Invoke();
            Destroy(gameObject);
        }
    }
}
