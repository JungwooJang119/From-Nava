using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadLineRenderer : MonoBehaviour
{
    [SerializeField] private GameObject NodeParent;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector2 offset;
    private Dictionary<int, Vector2> nodeToPos = new Dictionary<int, Vector2>();

    private void Awake() {
        int numChildren = NodeParent.transform.childCount;
        for(int i=0; i < numChildren; i++){
            Transform child = NodeParent.transform.GetChild(i);
            int nodeNum = child.GetComponentInChildren<NotepadNodeVisuals>().nodeNum;
            Vector2 pos = child.GetComponent<RectTransform>().localPosition;
            pos += offset;
            nodeToPos[nodeNum] = pos;
            print($"Node {nodeNum} X:{pos.x} Y:{pos.y}");
        }
    }


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
        //Set next LR point 
    }

    private void OnSpellCast(object sender, SpellType spellType)
    {
        //reset LR
    }
}
