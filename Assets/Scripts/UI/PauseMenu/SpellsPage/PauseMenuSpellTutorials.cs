using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class PauseMenuSpellTutorials : MonoBehaviour, ISpellPage {

    public event System.Action OnTutorialChanged;

    [SerializeField] private TextMeshProUGUI header, description, patternText;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private Image pattern;

    [SerializeField] private TutorialData[] tutorialOrder;
    public LinkedList<TutorialData> tutorialSequence;
    public LinkedListNode<TutorialData> head;

    public void OnEnable() {
        HashSet<ItemData> tutorialsClaimed = ReferenceSingleton.Instance.collectibleController.GetItems<TutorialData>();
        tutorialSequence = new LinkedList<TutorialData>(tutorialOrder.Where(tutorial => tutorialsClaimed.Contains(tutorial)));
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

    public void SetActiveTutorial(TutorialData tutorial) {
        header.text = tutorial.textHeader;
        description.text = tutorial.text;
        video.clip = tutorial.videoClip;
        pattern.sprite = tutorial.inputImage;
        patternText.text = tutorial.name == "Melee" ? "Input" : "Pattern";
        head = tutorialSequence.Find(tutorial);
        OnTutorialChanged?.Invoke();
    }
}
