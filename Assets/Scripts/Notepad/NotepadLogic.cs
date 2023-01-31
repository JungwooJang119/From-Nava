using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadLogic : Singleton<NotepadLogic>
{
    //Responsible for handling all logic related to casting spells
    public static event System.EventHandler<int> OnNodeSelected;

    //If spell is null, this is an invalid cast. If spell is not null, then this is a valid spell cast (of the param spell)
    public static event System.EventHandler<Spell> OnSpellCast;

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
        print("started pattern");
        AddToPattern(startNode);
        //sfx, vfx, etc
    }

    private void AddToPattern(int num) {
        pattern.Add(num);
        OnNodeSelected.Invoke(this, num);
        PrintPattern();
    }

    private void EndPattern() {
        AddToPattern(endNode);

        activePattern = false;
        //try to cast spell
        CompareSpellCast();
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
