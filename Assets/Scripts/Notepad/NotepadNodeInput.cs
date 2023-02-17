using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadNodeInput : NotepadInput
{
    public int nodeNum;

    public void OnClickButton() {
        base.InvokeInputPress(this, nodeNum);
    }
}
