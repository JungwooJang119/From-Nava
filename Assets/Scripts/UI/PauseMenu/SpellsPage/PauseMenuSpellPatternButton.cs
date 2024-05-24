using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuSpellPatternButton : MonoBehaviour {

    [SerializeField] private TutorialData tutorialData;
    public TutorialData TutorialData => tutorialData;

    private PauseMenuPage pageScript;
    private Image image;
    private TextMeshProUGUI text;
    private Button button;
    private Sprite originalSprite;
    private string originalText;
    private PauseMenuSpells masterScript;
    private PauseMenuSpellPatterns patternScript;
    private bool active;

    private Vector2 ogDelta;

    void Awake() {
        pageScript = GetComponentInParent<PauseMenuPage>(true);

        image = transform.GetChild(0).GetComponentInChildren<Image>(true);
        originalSprite = image.sprite;

        text = GetComponentInChildren<TextMeshProUGUI>(true);
        originalText = text.text;

        button = GetComponent<Button>();
        button.onClick.AddListener(ActivatePage);

        patternScript = GetComponentInParent<PauseMenuSpellPatterns>(true);
        masterScript = GetComponentInParent<PauseMenuSpells>(true);

        ogDelta = image.rectTransform.sizeDelta;
    }

    private void OnEnable() {
        active = ReferenceSingleton.Instance.collectibleController.GetItems<TutorialData>().Contains(tutorialData);
        if (active) {
            if (image.sprite != originalSprite) {
                image.rectTransform.sizeDelta = ogDelta;
                image.sprite = originalSprite;
            } if (pageScript.UpdateDictionary(image, 1f)) image.color = new Color (image.color.r, image.color.g, image.color.b, 1f);
            if (text.text != originalText) text.text = originalText;
            if (pageScript.UpdateDictionary(text, 1f)) text.alpha = 1f;
            if (!button.enabled) button.enabled = true;
        } else {
            text.text = "- - -";
            if (pageScript.UpdateDictionary(text, 0.5f)) text.alpha = 0.5f;
            image.sprite = patternScript.DisabledPattern;
            image.rectTransform.sizeDelta = patternScript.DisabledDelta;
            if (pageScript.UpdateDictionary(image, 0.5f)) image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            button.enabled = false;
        }
    }

    public void ActivatePage() {
        masterScript.SetActiveSpellPage(typeof(PauseMenuSpellTutorials), tutorialData);
    }

    public void Toggle(bool active) => this.active = active;
}
