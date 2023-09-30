using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TutorialDataBank;

public class PauseMenuSpellPatternButton : MonoBehaviour {

    [SerializeField] private TutorialType tutorialType;

    private PauseMenuPage pageScript;
    private Image image;
    private TextMeshProUGUI text;
    private Button button;
    private Sprite originalSprite;
    private string originalText;
    private PauseMenuSpells masterScript;
    private PauseMenuSpellPatterns patternScript;
    private bool active;

    void Awake() {
        pageScript = GetComponentInParent<PauseMenuPage>();

        image = transform.GetChild(0).GetComponentInChildren<Image>();
        originalSprite = image.sprite;

        text = GetComponentInChildren<TextMeshProUGUI>();
        originalText = text.text;

        button = GetComponent<Button>();
        button.onClick.AddListener(ActivatePage);

        patternScript = GetComponentInParent<PauseMenuSpellPatterns>();
        masterScript = GetComponentInParent<PauseMenuSpells>();
    }

    private void OnEnable() {
        active = ReferenceSingleton.Instance.collectibleController.GetTutorialList().Contains(tutorialType.ToString());
        if (active) {
            if (image.sprite != originalSprite) image.sprite = originalSprite;
            if (pageScript.UpdateDictionary(image, 1f)) image.color = new Color (image.color.r, image.color.g, image.color.b, 1f);
            if (text.text != originalText) text.text = originalText;
            if (pageScript.UpdateDictionary(text, 1f)) text.alpha = 1f;
            if (!button.enabled) button.enabled = true;
        } else {
            text.text = "- - -";
            if (pageScript.UpdateDictionary(text, 0.5f)) text.alpha = 0.5f;
            image.sprite = patternScript.GetDisabledPattern();
            if (pageScript.UpdateDictionary(image, 0.5f)) image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            button.enabled = false;
        }
    }

    public void ActivatePage() {
        masterScript.SetActiveSpellPage(typeof(PauseMenuSpellTutorials), tutorialType);
    }

    public TutorialType GetTutorialType() {
        return tutorialType;
    }

    public void Toggle(bool active) {
        this.active = active;
    }
}
