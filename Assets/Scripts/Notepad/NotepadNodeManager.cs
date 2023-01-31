using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadNodeManager : MonoBehaviour
{
    //At the highest level of a node. 
    public int nodeNum;

    private void OnEnable() {
        NotepadInput.OnInputPress += OnInputPress;
    }

    private void OnDisable() {
        NotepadInput.OnInputPress -= OnInputPress;
    }

    private void OnInputPress(object sender, int inputNum) {
        if(inputNum != nodeNum) return;

        //handle visuals
    }
}
