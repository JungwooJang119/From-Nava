using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Uses numpad in place of notepad nodes for inputting spells

public class NotepadNumpadInput : NotepadInput
{
    public int nodeNum;

    public void Update()
    {
        // we need to remap the keycodes because the numpad starts from the top left
        // of the keypad while the numbers start from the bottom left.
        Dictionary<int, KeyCode> keyMap = new Dictionary<int, KeyCode>() {
            {1, KeyCode.Keypad7},
            {2, KeyCode.Keypad8},
            {3, KeyCode.Keypad9},
            {4, KeyCode.Keypad4},
            {5, KeyCode.Keypad5},
            {6, KeyCode.Keypad6},
            {7, KeyCode.Keypad1},
            {8, KeyCode.Keypad2},
            {9, KeyCode.Keypad3},
            {0, KeyCode.Keypad0},
        };
        KeyCode key = keyMap[nodeNum];
        if (Input.GetKeyDown(key))
        {
            base.InvokeInputPress(this, nodeNum);
        }
    }
}
