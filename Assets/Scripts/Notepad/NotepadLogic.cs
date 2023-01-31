using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadLogic : Singleton<NotepadLogic>
{
    //Responsible for handling all logic related to casting spells

    [SerializeField] private int startNode;
    [SerializeField] private int endNode;

    [SerializeField] private List<Spell> spells = new List<Spell>();

    private List<int> pattern = new List<int>();
    private bool activePattern = false;

    private void Awake() {
        InitializeSingleton();
    }

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

        else if(activePattern && !pattern.Contains(inputNum))
            AddToPattern(inputNum);
        else {
            //invalid input: active node, non-start node on inactive pattern, etc
            //handle at will
            print("Inavlid node Clicked");
        }

    }

    private void StartPattern() {
        activePattern = true;
        pattern = new List<int>();
        pattern.Add(startNode);
        print("started pattern");
        //sfx, vfx, etc
    }

    private void AddToPattern(int num) {
        pattern.Add(num);

        PrintPattern();
    }

    private void EndPattern() {
        activePattern = false;
        pattern.Add(endNode);

        //try to cast spell
        CompareSpellCast();

        PrintPattern();
        ResetPattern();
        print("ended pattern");
        //sfx, vfx, etc
    }

    private void ResetPattern() {
        pattern = new List<int>();
    }

    private void CompareSpellCast()
    {
        //Check against all spells in list to see if a pattern matches a castable spell
    }

    private void CastSpell(Spell spell)
    {

    }

    private void OnInvalidPattern()
    {

    }


    private void PrintPattern() {
        string patternString = "";
        foreach (int num in pattern)
            patternString += num + ", ";
        print(patternString);
    }
}
