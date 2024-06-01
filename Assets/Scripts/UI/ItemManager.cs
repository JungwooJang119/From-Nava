using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ItemManager<T> : CollectibleManager<T> where T : CollectibleItem {

	[SerializeField] private float fadeRate;
	[SerializeField] private float flashRate;
    [SerializeField] private GameObject itemImage;
	[SerializeField] private GameObject itemText;
	[SerializeField] private GameObject bottomNote;

	private Image itemRenderer;
	private TextMeshProUGUI currentText;
	private TextMeshProUGUI currentNote;
	
	private readonly string intKey = "space";

    void Awake() {
		itemRenderer = itemImage.GetComponent<Image>();
		currentText = itemText.GetComponent<TextMeshProUGUI>();
		currentNote = bottomNote.GetComponent<TextMeshProUGUI>();
	}

    /// <summary>
    /// Won't be necessary with New Input System;
    /// </summary>
    void Update() {
		if (state == State.Await && Input.GetKeyDown(intKey)) state = State.End;
    }

    public override void Display(T itemData, float transitionTime = 0) {

		itemImage.SetActive(true);
		var itemImageTransform = itemImage.transform;
		itemImageTransform.SetParent(canvasTransform);
		itemImageTransform.localScale = Vector3.one * 0.8f;
		itemImageTransform.rotation = canvasTransform.rotation;
		itemImageTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 2, canvasTransform.position.z);
		itemRenderer.sprite = itemData.sprite;
		itemRenderer.SetNativeSize();

		itemText.SetActive(true);
		var itemTextTransform = itemText.transform;
		itemTextTransform.SetParent(canvasTransform);
		itemTextTransform.localScale = Vector3.one;
		itemTextTransform.rotation = canvasTransform.rotation;
		itemTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 5, canvasTransform.position.z);
		currentText.text = itemData.text;

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
					} state = State.Reveal;
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
					} yield return null;
					break;
			}
		}
	}

	private float MoveOpacity(float alpha, float rate) {
		Color targetColor = itemRenderer.color; targetColor.a = alpha;
		itemRenderer.color = Vector4.MoveTowards(itemRenderer.color, targetColor, Time.deltaTime * rate);
		currentText.alpha = Mathf.MoveTowards(currentText.alpha, alpha, Time.deltaTime * rate);
		currentNote.alpha = Mathf.MoveTowards(currentNote.alpha, alpha, Time.deltaTime * rate);
		return currentText.alpha;
	}

	private void SetOpacity(float alpha) {
		Color color = itemRenderer.color; color.a = alpha;
		itemRenderer.color = color;
		currentText.alpha = alpha;
		currentNote.alpha = alpha;
	}

	private void ResetElements() {
		itemImage.transform.SetParent(transform);
		itemImage.SetActive(false);

		itemText.transform.SetParent(transform);
		itemText.SetActive(false);

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
	}
}