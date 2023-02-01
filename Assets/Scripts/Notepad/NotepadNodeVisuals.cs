using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadNodeVisuals : MonoBehaviour
{
    //Handles all visual aspects of an individual notepad node

    public int nodeNum;

    private void OnEnable() {
        NotepadLogic.OnNodeSelected += OnNodeSelected;
        NotepadLogic.OnSpellCast += OnSpellCast;
    }

    private void OnDisable() {
        NotepadLogic.OnNodeSelected -= OnNodeSelected;
        NotepadLogic.OnSpellCast -= OnSpellCast;
    }

    private void OnNodeSelected(object sender, int num)
    {
        if(num != nodeNum) return;
        SelectNode();
    }

    private void SelectNode() {
        print($"Node {nodeNum} Selected");
    }

    private void OnSpellCast(object sender, SpellType spellType)
    {
        DeselectNode();
    }

    private void DeselectNode() {
        print($"Node {nodeNum} Deselected");
    }
}
