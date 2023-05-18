using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PolaroidManager : MonoBehaviour, ICollectibleManager
{
    public event Action OnDisplayEnd;

    private PolaroidDataBank polaroidDataBank;

    private enum State {
		Idle,
		Start,
		Fade,
        Await,
        End,
    } private State state = State.Idle;

	private GameObject polaroidImage;
	private GameObject polaroidText;
	private GameObject bottomNote;

	private Image polaroidRenderer;
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

	private bool firstDisplay;

	// Start is called before the first frame update
	void Start() {
		polaroidDataBank = GetComponent<PolaroidDataBank>();

		foreach (Transform t in transform) {
			if (t.gameObject.name == "PolaroidImage") polaroidImage = t.gameObject;
			if (t.gameObject.name == "PolaroidText") polaroidText = t.gameObject;
			if (t.gameObject.name == "BottomNote") bottomNote = t.gameObject;
		}

		transitionScript = ReferenceSingleton.Instance.transition;
		canvasTransform = transitionScript.transform.parent;
	}

    // Update is called once per frame
    void Update() {
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
					noteAlpha += Time.deltaTime;
				} else {
					noteAlpha -= Time.deltaTime;
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

		if (timer > 0) timer -= Time.deltaTime;
	}

    public void Display(string polaroid, float transitionTime = 0) {
        var polaroidData = polaroidDataBank.GetPolaroidData(polaroid);

		if (polaroidRenderer == null) {
			polaroidImage.SetActive(true);
			var polaroidImageTransform = polaroidImage.transform;
			polaroidImageTransform.SetParent(canvasTransform);
			polaroidImageTransform.localScale = Vector3.one * 0.8f;
			polaroidImageTransform.rotation = canvasTransform.rotation;
			polaroidImageTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 2, canvasTransform.position.z);
			polaroidRenderer = polaroidImage.GetComponent<Image>();
		} polaroidRenderer.sprite = polaroidData.sprite;
		polaroidRenderer.SetNativeSize();

		if (currentText == null) {
			polaroidText.SetActive(true);
			var polaroidTextTransform = polaroidText.transform;
			polaroidTextTransform.SetParent(canvasTransform);
			polaroidTextTransform.localScale = Vector3.one;
			polaroidTextTransform.rotation = canvasTransform.rotation;
			polaroidTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 5, canvasTransform.position.z);
			currentText = polaroidText.GetComponent<TextMeshProUGUI>();
		} currentText.text = polaroidData.text;

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
		rate *= Time.deltaTime;
		if (rate > 0) {
			textAlpha = Mathf.Min(1, textAlpha + rate);
			noteAlpha = Mathf.Min(1, textAlpha + rate);
		} else {
			textAlpha = Mathf.Max(0, textAlpha + rate);
			noteAlpha = Mathf.Max(0, textAlpha + rate);
		}
		currentText.color = new Color(_r, _g, _b, textAlpha);
		polaroidRenderer.color = currentText.color;
		currentNote.color = new Color(_r, _g, _b, noteAlpha);
	}

	private void ResetElements() {
		polaroidImage.transform.SetParent(transform);
		polaroidImage.SetActive(false);
		polaroidRenderer = null;

		polaroidText.transform.SetParent(transform);
		polaroidText.SetActive(false);
		currentText = null;

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
		currentNote = null;
	}
}