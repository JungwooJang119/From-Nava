using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadLogic : MonoBehaviour
{
    //Responsible for handling all logic related to casting spells

    [SerializeField] private int startNode;
    [SerializeField] private int endNode;

    [SerializeField] private List<Spell> spells = new List<Spell>();

    private string patternString = "";
    private bool activePattern = false;

    private void OnEnable() {
        NotepadInput.OnInputPress += OnInputPress;
    }

    private void OnDisable() {
        NotepadInput.OnInputPress -= OnInputPress;
    }

    private void OnInputPress(object sender, int inputNum) {
        if(!activePattern && inputNum == startNode) 
            StartPattern();
        
        else if(activePattern && inputNum == endNode)
            EndPattern();

        else if(activePattern && !patternString.Contains((char)(inputNum - '0')))
            AddToPattern(inputNum);

    }

    private void StartPattern() {
        activePattern = true;
        patternString = startNode.ToString();

        print("started pattern");
        //sfx, vfx, etc
    }

    private void AddToPattern(int num) {
        patternString += num.ToString();

        print($"Pattern: {patternString}");
    }

    private void EndPattern() {
        activePattern = false;
        patternString += endNode.ToString();

        //try to cast spell

        print($"Pattern: {patternString}");
        //sfx, vfx, etc
    }

    private void ResetPattern() {
        patternString = "";
    }

    private void compareSpellCast()
    {

    }

    private void CastSpell(Spell spell)
    {

    }

    private void OnInvalidPattern()
    {

    }

}
