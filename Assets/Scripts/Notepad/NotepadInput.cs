using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NotepadInput : MonoBehaviour
{
    //Base class for all notepad input types. 
    public static event System.EventHandler<int> OnInputPress;

    protected void InvokeInputPress(Object sender, int nodeNum)
    {
        OnInputPress?.Invoke(sender, nodeNum);
    }

}
