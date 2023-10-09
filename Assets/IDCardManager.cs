using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDCardManager : MonoBehaviour, ICollectibleManager
{
    public event Action OnDisplayEnd;

    private IDCardDataBank idCardDataBank;

    private enum State {
		Idle,
		Start,
		Fade,
        Await,
        End,
    } private State state = State.Idle;

	private GameObject idCardImage;
	private GameObject idCardText;
	private GameObject bottomNote;

	private Image idCardRenderer;
	private TextMeshProUGUI currentText;
	private TextMeshProUGUI currentNote;

	private string intKey = "space";
	
    private Transform canvasTransform;
	private tranMode transitionScript;
	
    private float timer = 0;

	// Text color and opacity;
	private byte _r = 255, _g = 255, _b = 255; // RGB values for the text color;
	private float textAlpha = 0;              // Controls alpha fadeout of text;
	private float noteAlpha = 0;                // Second alpha for the bottom alert text;
	private bool alertUp;

	private float deltaTime = 0.01f; 

	// Start is called before the first frame update
	void Start() {
		idCardDataBank = GetComponent<IDCardDataBank>();

		foreach (Transform t in transform) {
			if (t.gameObject.name == "IDCardImage") idCardImage = t.gameObject;
			if (t.gameObject.name == "IDCardText") idCardText = t.gameObject;
			if (t.gameObject.name == "BottomNote") bottomNote = t.gameObject;
		}

		transitionScript = ReferenceSingleton.Instance.transition;
		canvasTransform = transitionScript.transform.parent;
	}

    // Update is called once per frame
    void Update() {
		deltaTime = Time.deltaTime > 0 ? Time.deltaTime : 0.01f;

		switch (state) {
			case State.Start:
				if (timer <= 0) {
					AudioControl.Instance.PlayVoidSFX("Woosh", 0, 0.75f);
					state = State.Fade;
				} break;

			case State.Fade:
				if (textAlpha < 1) {
					ChangeOpacity(2f);
				} else {
					state = State.Await;
				} break;

			case State.Await:
				if (noteAlpha >= 1) {
					alertUp = false;
				} else if (noteAlpha <= 0.1) {
					alertUp = true;
				} 
				if (alertUp) {
					noteAlpha += deltaTime;
				} else {
					noteAlpha -= deltaTime;
				} currentNote.color = new Color(_r, _g, _b, noteAlpha);

				if (Input.GetKeyDown(intKey)) {
					state = State.End;
				} break;

			case State.End:
				if (textAlpha > 0) {
					ChangeOpacity(-4f);
				} else {
					OnDisplayEnd?.Invoke();
					ResetElements();
					state = State.Idle;
				}
				break;
		}

		if (timer > 0) timer -= deltaTime;
	}

    public void Display(string idCard, float transitionTime = 0) {
        var idCardData = idCardDataBank.GetIDCardData(idCard);

		if (idCardRenderer == null) {
			idCardImage.SetActive(true);
			var idCardImageTransform = idCardImage.transform;
			idCardImageTransform.SetParent(canvasTransform);
			idCardImageTransform.localScale = Vector3.one * 0.8f;
			idCardImageTransform.rotation = canvasTransform.rotation;
			idCardImageTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 2, canvasTransform.position.z);
			idCardRenderer = idCardImage.GetComponent<Image>();
		} idCardRenderer.sprite = idCardData.sprite;
		idCardRenderer.SetNativeSize();

		if (currentText == null) {
			idCardText.SetActive(true);
			var idCardTextTransform = idCardText.transform;
			idCardTextTransform.SetParent(canvasTransform);
			idCardTextTransform.localScale = Vector3.one;
			idCardTextTransform.rotation = canvasTransform.rotation;
			idCardTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 5, canvasTransform.position.z);
			currentText = idCardText.GetComponent<TextMeshProUGUI>();
		} currentText.text = idCardData.text;

		if (currentNote == null) {
			bottomNote.SetActive(true);
			var bottomNoteTransform = bottomNote.transform;
			bottomNoteTransform.SetParent(canvasTransform);
			bottomNoteTransform.localScale = Vector3.one;
			bottomNoteTransform.rotation = canvasTransform.rotation;
			bottomNoteTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 7, canvasTransform.position.z);
			currentNote = bottomNote.GetComponent<TextMeshProUGUI>();
			currentNote.text = "Advance [" + intKey.ToUpper() + "]";
		}

		if (transitionTime > 0) {
			timer = transitionTime;
			state = State.Start;
		} else {
			state = State.Fade;
			// AudioControl.PlayVoidSFX("Paper Drawn");
		} textAlpha = 0;
		noteAlpha = 0;
		ChangeOpacity(0);
	}

	private void ChangeOpacity(float rate) {
		rate *= deltaTime;
		if (rate > 0) {
			textAlpha = Mathf.Min(1, textAlpha + rate);
			noteAlpha = Mathf.Min(1, textAlpha + rate);
		} else {
			textAlpha = Mathf.Max(0, textAlpha + rate);
			noteAlpha = Mathf.Max(0, textAlpha + rate);
		}
		currentText.color = new Color(_r, _g, _b, textAlpha);
		idCardRenderer.color = currentText.color;
		currentNote.color = new Color(_r, _g, _b, noteAlpha);
	}

	private void ResetElements() {
		idCardImage.transform.SetParent(transform);
		idCardImage.SetActive(false);
		idCardRenderer = null;

		idCardText.transform.SetParent(transform);
		idCardText.SetActive(false);
		currentText = null;

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
		currentNote = null;
	}
}
