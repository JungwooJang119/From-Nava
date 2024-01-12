using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SideRoomKeyManager : CollectibleManager {

    private SideRoomKeyDataBank sideRoomKeyDataBank;

    private enum State {
		Idle,
		Start,
		Fade,
        Await,
        End,
    } private State state = State.Idle;

	private GameObject sideRoomKeyImage;
	private GameObject sideRoomKeyText;
	private GameObject bottomNote;

	private Image sideRoomKeyRenderer;
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
		sideRoomKeyDataBank = GetComponent<SideRoomKeyDataBank>();

		foreach (Transform t in transform) {
			if (t.gameObject.name == "SideRoomKeyImage") sideRoomKeyImage = t.gameObject;
			if (t.gameObject.name == "SideRoomKeyText") sideRoomKeyText = t.gameObject;
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
					InvokeOnDisplayEnd();
					ResetElements();
					state = State.Idle;
				}
				break;
		}

		if (timer > 0) timer -= deltaTime;
	}

    public void Display(string sideRoomKey, float transitionTime = 0) {
        var sideRoomKeyData = sideRoomKeyDataBank.GetSideRoomKeyData(sideRoomKey);

		if (sideRoomKeyRenderer == null) {
			sideRoomKeyImage.SetActive(true);
			var sideRoomKeyImageTransform = sideRoomKeyImage.transform;
			sideRoomKeyImageTransform.SetParent(canvasTransform);
			sideRoomKeyImageTransform.localScale = Vector3.one * 0.8f;
			sideRoomKeyImageTransform.rotation = canvasTransform.rotation;
			sideRoomKeyImageTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 2, canvasTransform.position.z);
			sideRoomKeyRenderer = sideRoomKeyImage.GetComponent<Image>();
		} sideRoomKeyRenderer.sprite = sideRoomKeyData.sprite;
		sideRoomKeyRenderer.SetNativeSize();

		if (currentText == null) {
			sideRoomKeyText.SetActive(true);
			var sideRoomKeyTextTransform = sideRoomKeyText.transform;
			sideRoomKeyTextTransform.SetParent(canvasTransform);
			sideRoomKeyTextTransform.localScale = Vector3.one;
			sideRoomKeyTextTransform.rotation = canvasTransform.rotation;
			sideRoomKeyTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 5, canvasTransform.position.z);
			currentText = sideRoomKeyText.GetComponent<TextMeshProUGUI>();
		} currentText.text = sideRoomKeyData.text;

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
		sideRoomKeyRenderer.color = currentText.color;
		currentNote.color = new Color(_r, _g, _b, noteAlpha);
	}

	private void ResetElements() {
		sideRoomKeyImage.transform.SetParent(transform);
		sideRoomKeyImage.SetActive(false);
		sideRoomKeyRenderer = null;

		sideRoomKeyText.transform.SetParent(transform);
		sideRoomKeyText.SetActive(false);
		currentText = null;

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
		currentNote = null;
	}
}
