using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManager : CollectibleManager<TutorialData> {

	[SerializeField] private float fadeRate;
	[SerializeField] private float flashRate;
	[SerializeField] private GameObject itemImage;
	[SerializeField] private GameObject itemText;
	[SerializeField] private GameObject bottomNote;
	[SerializeField] private GameObject tutorialVideo;

	private Image itemRenderer;
	private TextMeshProUGUI currentText;
	private TextMeshProUGUI currentNote;
	private VideoPlayer videoRenderer;
	private RawImage videoTexture;

	private readonly string intKey = "space";

	void Awake() {
		itemRenderer = itemImage.GetComponent<Image>();
		currentText = itemText.GetComponent<TextMeshProUGUI>();
		currentNote = bottomNote.GetComponent<TextMeshProUGUI>();
		videoRenderer = tutorialVideo.GetComponent<VideoPlayer>();
		videoTexture = tutorialVideo.GetComponent<RawImage>();
	}

	/// <summary>
	/// Won't be necessary with New Input System;
	/// </summary>
	void Update() {
		if (state == State.Await && Input.GetKeyDown(intKey)) state = State.End;
	}

	public override void Display(TutorialData itemData, float transitionTime = 0) {

		if (itemData.videoClip != null) {
			tutorialVideo.SetActive(true);
			var tutorialGIFTransform = tutorialVideo.transform;
			tutorialGIFTransform.SetParent(canvasTransform);
			tutorialGIFTransform.localScale = Vector3.one * 0.8f;
			tutorialGIFTransform.rotation = canvasTransform.rotation;
			tutorialGIFTransform.position = new Vector3(canvasTransform.position.x + 4, canvasTransform.position.y - 3, canvasTransform.position.z);
			videoRenderer.clip = itemData.videoClip;
		}

		if (itemData.inputImage != null) {
			itemImage.SetActive(true);
			var inputImageTransform = itemImage.transform;
			inputImageTransform.SetParent(canvasTransform);
			inputImageTransform.localScale = Vector3.one * 1.2f;
			inputImageTransform.rotation = canvasTransform.rotation;
			inputImageTransform.position = new Vector3(canvasTransform.position.x - 6, canvasTransform.position.y - 3, canvasTransform.position.z);
			itemRenderer.sprite = itemData.inputImage;
			itemRenderer.SetNativeSize();
		}

		itemText.SetActive(true);
		var tutorialTextTransform = itemText.transform;
		tutorialTextTransform.SetParent(canvasTransform);
		tutorialTextTransform.localScale = Vector3.one;
		tutorialTextTransform.rotation = canvasTransform.rotation;
		if (itemData.inputImage != null) {
			tutorialTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 3, canvasTransform.position.z);
		} else tutorialTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 1, canvasTransform.position.z);
		currentText.text = (transitionTime > 0 ? itemData.textHeaderNew : itemData.textHeader) + "\n\n" + itemData.text;

		bottomNote.SetActive(true);
		var bottomNoteTransform = bottomNote.transform;
		bottomNoteTransform.SetParent(canvasTransform);
		bottomNoteTransform.localScale = Vector3.one;
		bottomNoteTransform.rotation = canvasTransform.rotation;
		bottomNoteTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 7, canvasTransform.position.z);
		currentNote.text = "Advance [" + intKey.ToUpper() + "]";

		StopAllCoroutines();
		state = State.Start;
		SetOpacity(0);
		StartCoroutine(DisplayAsync(transitionTime));
	}

	private IEnumerator DisplayAsync(float transitionTime) {

		float cycleTarget = 0;
		while (state != State.Idle) {
			float alpha;
			switch (state) {
				case State.Start:
					if (transitionTime > 0) {
						yield return new WaitForSeconds(transitionTime);
						AudioControl.Instance.PlayVoidSFX("Woosh", 0, 0.75f);
					}
					state = State.Reveal;
					break;
				case State.Reveal:
					alpha = MoveOpacity(1, fadeRate);
					if (alpha >= 1) state = State.Await;
					yield return null;
					break;
				case State.Await:
					currentNote.alpha = Mathf.MoveTowards(currentNote.alpha, cycleTarget,
														  Time.deltaTime * flashRate * (cycleTarget == 0 ? 1 : 2));
					if (Mathf.Approximately(currentNote.alpha, cycleTarget)) cycleTarget = cycleTarget == 0 ? 1 : 0;
					yield return null;
					break;
				case State.End:
					alpha = MoveOpacity(0, fadeRate * 1.5f);
					if (alpha <= 0) {
						ResetElements();
						controller.Poke();
						state = State.Idle;
					}
					yield return null;
					break;
			}
		}
	}

	private float MoveOpacity(float alpha, float rate) {
		Color targetColor = itemRenderer.color; targetColor.a = alpha;
		videoTexture.color = Vector4.MoveTowards(videoTexture.color, targetColor, Time.deltaTime * rate);
		itemRenderer.color = Vector4.MoveTowards(itemRenderer.color, targetColor, Time.deltaTime * rate);
		currentText.alpha = Mathf.MoveTowards(currentText.alpha, alpha, Time.deltaTime * rate);
		currentNote.alpha = Mathf.MoveTowards(currentNote.alpha, alpha, Time.deltaTime * rate);
		return currentText.alpha;
	}

	private void SetOpacity(float alpha) {
		Color color = itemRenderer.color; color.a = alpha;
		videoTexture.color = color;
		itemRenderer.color = color;
		currentText.alpha = alpha;
		currentNote.alpha = alpha;
	}

	private void ResetElements() {
		tutorialVideo.transform.SetParent(transform);
		tutorialVideo.SetActive(false);

		itemImage.transform.SetParent(transform);
		itemImage.SetActive(false);

		itemText.transform.SetParent(transform);
		itemText.SetActive(false);

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
	}
}