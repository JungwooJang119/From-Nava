using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotepadLogic : Singleton<NotepadLogic>
{
    //Responsible for handling all logic related to casting spells
    public static event System.EventHandler<int> OnNodeSelected;

    //Fired when there is a valid spell cast
    public static event System.EventHandler<SpellType> OnSpellCast;

    [SerializeField] private int startNode;
    [SerializeField] private int endNode;

    [Serializable]
    public struct SpellData
    {
        public SpellType spellType;
        public string pattern; //C: only a string here because it makes the editor cleaner
        public bool unlocked;
    }

    [SerializeField] private List<SpellData> spellData;


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
            print("Invalid node Clicked");
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
        string patternString = GetPatternString();
        foreach (SpellData sd in spellData) {
            if(sd.unlocked && patternString.Equals(sd.pattern)) 
            {
                OnSpellCast?.Invoke(this, sd.spellType);
                return;
            }
        }
        OnInvalidPattern();
    }

    private void OnInvalidPattern()
    {
        print("Invalid Pattern");
        OnSpellCast?.Invoke(this, SpellType.NONE);
    }

    public void UnlockSpell(SpellType spellType)
    {
        for(int i = 0; i < spellData.Count; i++) {
            SpellData sd = spellData[i];
            if(spellType == sd.spellType) 
            {
                sd.unlocked = true;
                return;
            }
        }
    }


    private void PrintPattern() {
        string patternString = "";
        foreach (int num in pattern)
            patternString += num + ", ";
        print(patternString);
    }

    private string GetPatternString()
    {
        string result = "";
        foreach(int num in pattern)
            result += pattern.ToString();
        return result;
    }
}