using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadNodeVisuals : MonoBehaviour
{
    //Handles all visual aspects of an individual notepad node

    public int nodeNum;
    [SerializeField] private float nodeHighlightTime;

    [SerializeField] private GameObject unselected;
    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject goodOutline;
    [SerializeField] private GameObject badOutline;

    private void Awake() {
        DeselectNode();
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
        if(num != nodeNum) return;
        SelectNode();
    }

    private void SelectNode() {
        selected?.SetActive(true);
        unselected?.SetActive(false);
        print($"Node {nodeNum} Selected");
    }

    private void OnSpellCast(object sender, NotepadLogic.OnSpellCastArgs e)
    {
        if(e.spellType == SpellType.NONE)
            StartCoroutine(OnFail());
        else
            StartCoroutine(OnSuccess());
    }

    private IEnumerator OnSuccess() {
        goodOutline?.SetActive(true);
        yield return new WaitForSeconds(nodeHighlightTime);
        goodOutline?.SetActive(false);
        DeselectNode();
    }

    private IEnumerator OnFail() {
        badOutline?.SetActive(true);
        yield return new WaitForSeconds(nodeHighlightTime);
        badOutline?.SetActive(false);
        DeselectNode();
    }

    private void DeselectNode() {
        selected?.SetActive(false);
        unselected?.SetActive(true);
        badOutline?.SetActive(false);
        badOutline?.SetActive(false);
    }
}
