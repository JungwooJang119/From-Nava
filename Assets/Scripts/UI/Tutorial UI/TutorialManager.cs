using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManager : MonoBehaviour, ICollectibleManager
{
	public event Action OnTutorialEnd;

	private TutorialDataBank tutorialDataBank;

	private enum State {
		Idle,
		Start,
		Fade,
		Await,
		End,
	} private State state = State.Idle;

	private GameObject tutorialVideo;
	private GameObject inputImage;
	private GameObject tutorialText;
	private GameObject bottomNote;

	private VideoPlayer videoRenderer;
	private RawImage videoTexture;
	private Image inputRenderer;
	private TextMeshProUGUI currentText;
	private TextMeshProUGUI currentNote;

	private string intKey = "space";

	private Transform canvasTransform;
	private tranMode transitionScript;

	private float timer = 0;

	// Text color and opacity;
	private byte _r = 255, _g = 255, _b = 255; // RGB values for the text color;
	private byte textAlpha = 0;              // Controls alpha fadeout of text;
	private byte noteAlpha = 0;                // Second alpha for the bottom alert text;
	private bool alertUp;

	// Start is called before the first frame update
	void Start() {
		tutorialDataBank = GetComponent<TutorialDataBank>();

		foreach (Transform t in transform) {
			if (t.gameObject.name == "TutorialVideo") tutorialVideo = t.gameObject;
			if (t.gameObject.name == "InputImage") inputImage = t.gameObject;
			if (t.gameObject.name == "TutorialText") tutorialText = t.gameObject;
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
					state = State.Fade;
				} break;

			case State.Fade:
				if (textAlpha < 255) {
					ChangeOpacity(5);
				} else {
					state = State.Await;
				} break;

			case State.Await:
				if (noteAlpha >= 250) {
					alertUp = false;
				} else if (noteAlpha <= 50) {
					alertUp = true;
				}
				if (alertUp) {
					noteAlpha++;
				} else {
					noteAlpha--;
				}
				currentNote.color = new Color32(_r, _g, _b, noteAlpha);

				if (Input.GetKeyDown(intKey)) {
					state = State.End;
				} break;

			case State.End:
				if (textAlpha > 0) {
					ChangeOpacity(-5);
				} else {
					OnTutorialEnd?.Invoke();
					ResetElements();
					state = State.Idle;
				} break;
		}

		if (timer > 0) timer -= Time.deltaTime;
	}

	public void Display(string name, float transitionTime = 0) {
		var tutorialData = tutorialDataBank.GetTutorialData(name);

		if (tutorialData.videoClip != null) {
			if (videoRenderer == null) {
				tutorialVideo.SetActive(true);
				var tutorialGIFTransform = tutorialVideo.transform;
				tutorialGIFTransform.SetParent(canvasTransform);
				tutorialGIFTransform.localScale = Vector3.one * 0.8f;
				tutorialGIFTransform.rotation = canvasTransform.rotation;
				tutorialGIFTransform.position = new Vector3(canvasTransform.position.x + 5, canvasTransform.position.y - 3, canvasTransform.position.z);
				videoRenderer = tutorialVideo.GetComponent<VideoPlayer>();
				if (!videoTexture) videoTexture = tutorialVideo.GetComponent<RawImage>();
			} videoRenderer.clip = tutorialData.videoClip;
		}

		if (tutorialData.inputImage != null) {
			if (inputRenderer == null) {
				inputImage.SetActive(true);
				var inputImageTransform = inputImage.transform;
				inputImageTransform.SetParent(canvasTransform);
				inputImageTransform.localScale = Vector3.one * 1.2f;
				inputImageTransform.rotation = canvasTransform.rotation;
				inputImageTransform.position = new Vector3(canvasTransform.position.x - 4, canvasTransform.position.y - 3, canvasTransform.position.z);
				inputRenderer = inputImage.GetComponent<Image>();
			} inputRenderer.sprite = tutorialData.inputImage;
			inputRenderer.SetNativeSize();
		}

		if (currentText == null) {
			tutorialText.SetActive(true);
			var tutorialTextTransform = tutorialText.transform;
			tutorialTextTransform.SetParent(canvasTransform);
			tutorialTextTransform.localScale = Vector3.one;
			tutorialTextTransform.rotation = canvasTransform.rotation;
			if (tutorialData.inputImage != null) {
				tutorialTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 3, canvasTransform.position.z);
			} else {
				tutorialTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 1, canvasTransform.position.z);
			}
			currentText = tutorialText.GetComponent<TextMeshProUGUI>();
		} currentText.text = (transitionTime > 0 ? tutorialData.textHeaderNew : tutorialData.textHeader) + "\n\n" + tutorialData.text;

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

		timer = transitionTime;
		textAlpha = 0;
		noteAlpha = 0;
		ChangeOpacity(0);
		state = State.Start;
	}

	private void ChangeOpacity(int rate) {
		if (rate > 0) {
			textAlpha = (byte)Mathf.Min(255, (int)textAlpha + rate);
			noteAlpha = (byte)Mathf.Min(255, (int)textAlpha + rate);
		} else {
			textAlpha = (byte)Mathf.Max(0, (int)textAlpha + rate);
			noteAlpha = (byte)Mathf.Max(0, (int)textAlpha + rate);
		}
		currentText.color = new Color32(_r, _g, _b, textAlpha);
		if (videoTexture != null) videoTexture.color = currentText.color;
		if (inputRenderer != null) inputRenderer.color = currentText.color;
		currentNote.color = new Color32(_r, _g, _b, noteAlpha);
	}

	private void ResetElements() {
		tutorialVideo.transform.SetParent(transform);
		tutorialVideo.SetActive(false);
		videoRenderer = null;

		inputImage.transform.SetParent(transform);
		inputImage.SetActive(false);
		inputRenderer = null;

		tutorialText.transform.SetParent(transform);
		tutorialText.SetActive(false);
		currentText = null;

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
		currentNote = null;
	}
}