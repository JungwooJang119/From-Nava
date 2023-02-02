using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadLineRenderer : MonoBehaviour
{
    [SerializeField] private GameObject NodeParent;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float scale;
    [SerializeField] private float waitTime;
    private Dictionary<int, Vector2> nodeToPos = new Dictionary<int, Vector2>();

    private void Awake() {
        int numChildren = NodeParent.transform.childCount;
        for(int i=0; i < numChildren; i++){
            Transform child = NodeParent.transform.GetChild(i);
            if(!child.gameObject.activeSelf) break;
            int nodeNum = child.GetComponentInChildren<NotepadNodeVisuals>().nodeNum;
            Vector2 pos = child.GetComponent<RectTransform>().localPosition;
            pos += offset;
            pos /= scale;
            nodeToPos[nodeNum] = pos;
        }

        ResetLR();
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
        AddLRPoint(num);
    }

    private void AddLRPoint(int nodeNum)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, nodeToPos[nodeNum]);
    }

    private void OnSpellCast(object sender, NotepadLogic.OnSpellCastArgs e)
    {
        StartCoroutine(DelayLRReset());
    }

    private IEnumerator DelayLRReset()
    {
        yield return new WaitForSeconds(waitTime);
        ResetLR();
    }

    private void ResetLR()
    {
        lineRenderer.positionCount = 0;
    }
}
