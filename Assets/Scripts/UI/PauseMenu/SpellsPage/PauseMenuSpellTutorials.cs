using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;
using static TutorialDataBank;

public class PauseMenuSpellTutorials : MonoBehaviour, ISpellPage {

    public event Action OnTutorialChanged;

    [SerializeField] private TextMeshProUGUI header, description, patternText;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private Image pattern;

    private Dictionary<TutorialType, TutorialData> tutorialDict;
    public LinkedList<TutorialType> tutorialSequence;
    public LinkedListNode<TutorialType> head;

    void Awake() {
        tutorialDict = ReferenceSingleton.Instance.collectibleController.GetComponentInChildren<TutorialDataBank>().GetTutorialDict();
    }

    public void OnEnable() {
        tutorialSequence = new LinkedList<TutorialType>();
        var tutorialsClaimed = ReferenceSingleton.Instance.collectibleController.GetTutorialList();
        TutorialType parsedType = 0;
        foreach (string typeName in Enum.GetNames(typeof(TutorialType))) {
            if (typeName != "Basics" && tutorialsClaimed.Contains(typeName)) {
                if (Enum.TryParse(typeName, out parsedType)) tutorialSequence.AddLast(parsedType);
            }
        }
    }

    public bool GetPreviousTutorial(bool set = false) {
        var hasPrevious = head.Previous != null;
        if (hasPrevious && set) {
            var headPreviousValue = head.Previous.Value;
            head = head.Previous;
            hasPrevious = head.Previous != null;
            SetActiveTutorial(headPreviousValue);
        } return hasPrevious;
    }

    public bool GetNextTutorial(bool set = false) {
        var hasNext = head.Next != null;
        if (hasNext && set) {
            var headNextValue = head.Next.Value;
            head = head.Next;
            hasNext = head.Next != null;
            SetActiveTutorial(headNextValue);
        } return hasNext;
    }

    public void SetActiveTutorial(TutorialType type) {
        var tutorial = tutorialDict[type];
        header.text = tutorial.textHeader;
        description.text = tutorial.text;
        video.clip = tutorial.videoClip;
        pattern.sprite = tutorial.inputImage;
        patternText.text = type == TutorialType.Melee ? "Input" : "Pattern";
        head = tutorialSequence.Find(type);
        OnTutorialChanged?.Invoke();
    }
}
