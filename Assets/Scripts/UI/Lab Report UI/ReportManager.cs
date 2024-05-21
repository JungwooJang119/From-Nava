using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportManager : CollectibleManager<ReportData> {

	[SerializeField] private float fadeRate;
	[SerializeField] private float flashRate;
	[SerializeField] private GameObject reportText;
	[SerializeField] private GameObject bottomNote;

	private TextMeshProUGUI currentText;
	private TextMeshProUGUI currentNote;

	/// <summary> Listing holding page strings from a ReportData object; </summary>
	private readonly List<string> strings2Grab = new();
	/// <summary> Active string, successively modified during display; </summary>
	private string string2Report;

	/// <summary> Timer that enforces delay between key introductions; </summary>
	private float writeTimer = 0;
	private bool CanWrite => writeTimer <= 0;

	/// <summary> Controls how fast are characters written to the screen; </summary>
	private readonly float letterWait = 0.05f;
	/// <summary> Controls the current limit of the written string; </summary>
	private int currentIndex;

	/// <summary> Array with the names of key sounds; </summary>
	private readonly string[] soundStrings = { "Report Key v1", "Report Key v2" };

	// Legacy Input variables
	private string intKey = "space";
	private bool waitForPress = false;

	void Awake() {
		currentText = reportText.GetComponent<TextMeshProUGUI>();
		currentNote = bottomNote.GetComponent<TextMeshProUGUI>();
	}

	/// <summary>
	/// Won't be necessary with New Input System;
	/// </summary>
	void Update() {
		if (state == State.Reveal && Input.GetKeyDown(intKey)) {
			state = State.Await;
			AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0, 2)], 0.2f);
			currentText.text = BuildStr(string2Report, "<color=#00000000>|</color>");
			currentIndex = string2Report.Length;
			writeTimer = 0;
			waitForPress = true;
		}
		if (state == State.Await && Input.GetKeyUp(intKey)) {
			if (currentIndex < string2Report.Length) {
				state = State.Reveal;
				string2Report = string2Report.Substring(0, currentIndex) + "<color=#00000000>|</color>"
								 + string2Report.Substring(currentIndex);
				currentIndex += 31; /// Skip length of tag + length of escape sequence;
			} else if (!waitForPress) {
				state = State.End;
			} else {
				waitForPress = false;
			} writeTimer = 0;
		}
	}

	public override void Display(ReportData reportData, float transitionTime = 0) {
		strings2Grab.Clear();
		foreach (string report in reportData.textEntries) {
			strings2Grab.Add(report);
		} FetchString();

		reportText.SetActive(true);
		var reportTextTransform = reportText.transform;
		reportTextTransform.SetParent(canvasTransform);
		reportTextTransform.localScale = Vector3.one;
		reportTextTransform.rotation = canvasTransform.rotation;
		reportTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 0.75f, canvasTransform.position.z);
		currentText.text = "<color=#00000000>" + string2Report + "</color>";

		bottomNote.SetActive(true);
		var bottomNoteTransform = bottomNote.transform;
		bottomNoteTransform.SetParent(canvasTransform);
		bottomNoteTransform.localScale = Vector3.one;
		bottomNoteTransform.rotation = canvasTransform.rotation;
		bottomNoteTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 7, canvasTransform.position.z);
		currentNote.text = "Advance [" + intKey.ToUpper() + "]";

		StopAllCoroutines();
		state = State.Start;
		SetOpacity(1, 0);
		StartCoroutine(DisplayAsync(transitionTime));
	}

	private IEnumerator DisplayAsync(float transitionTime) {
		float cycleTarget = 0;
		bool dotted = false;
		while (state != State.Idle) {
			if (state == State.Reveal || state == State.Await) {
				currentNote.alpha = Mathf.MoveTowards(currentNote.alpha, cycleTarget,
									  Time.deltaTime * flashRate * (cycleTarget == 0 ? 1 : 2));
				if (Mathf.Approximately(currentNote.alpha, cycleTarget)) cycleTarget = cycleTarget == 0 ? 1 : 0;
			} float alpha;
			switch (state) {
				case State.Start:
					while (transitionTime > 0) {
						transitionTime = Mathf.MoveTowards(transitionTime, 0, Time.deltaTime);
						yield return null;
					} state = State.Reveal;
					break;
				case State.Reveal:
					if (CanWrite) {
						/// Move the start of the invisible color tag toward the end, thus showing more characters;
						if (currentIndex < string2Report.Length && string2Report[currentIndex] != '\\') {
							AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0, 2)], 0.15f);
							currentText.text = BuildStr(string2Report, "|");
							writeTimer = letterWait;
							currentIndex++;
						} else if (currentIndex < string2Report.Length && string2Report[currentIndex] == '\\') {
							state = State.Await;
							currentText.text = BuildStr(string2Report, "|");
							writeTimer = 0;
						} else if (currentIndex == string2Report.Length) {
							state = State.Await;
							writeTimer = 0;
						}
					} yield return null;
					break;
				case State.Await:
					/// Intermitent flashing of the pointer character;
					if (CanWrite) {
						if (dotted) {
							currentText.text = BuildStr(string2Report, "|");
							writeTimer = 0.5f;
							dotted = false;
						} else {
							currentText.text = BuildStr(string2Report, "<color=#00000000>|</color>");
							writeTimer = 0.5f;
							dotted = true;
						}
					} yield return null;
					break;
				case State.End:
					alpha = MoveOpacity(0, fadeRate * 1.75f);
					if (alpha == 0) {
						if (FetchString()) {
							NextPage();
							currentIndex = 0;
							writeTimer = 0.5f;
							state = State.Reveal;
						} else {
							ResetElements();
							controller.Poke();
							state = State.Idle;
						}
					} yield return null;
					break;
			} writeTimer = Mathf.MoveTowards(writeTimer, 0, Time.deltaTime);
		}
	}

	private void NextPage() {
		currentText.text = "<color=#00000000>" + string2Report + "</color>";
	}

	///<summary>
	/// Builds the string as if it were typed in real-time.
	///</summary>
	private string BuildStr(string str, string lineAppend) {
		string nstr = (str.Substring(0, currentIndex) + lineAppend
									+ "<color=#00000000>" + str.Substring(currentIndex) + "</color>")
									.Replace("\\n", "\n"); /// Fix for a weird string parsing issue with Unity;
		return nstr;
	}

	///<summary> 
	/// Determines whether there are more pages to display, and if so changes the current string;
	///</summary>
	private bool FetchString() {
		if (strings2Grab.Count > 0) {
			string2Report = strings2Grab[0];
			strings2Grab.RemoveAt(0);
			return true;
		} return false;
	}

	private float MoveOpacity(float alpha, float rate) {
		currentText.alpha = Mathf.MoveTowards(currentText.alpha, alpha, Time.deltaTime * rate);
		currentNote.alpha = Mathf.MoveTowards(currentNote.alpha, alpha, Time.deltaTime * rate);
		return currentText.alpha;
	}

	private void SetOpacity(float textAlpha, float noteAlpha) {
		currentText.alpha = textAlpha;
		currentNote.alpha = noteAlpha;
	}

	private void ResetElements() {
		currentIndex = 0;

		reportText.transform.SetParent(transform);
		reportText.SetActive(false);

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
	}
}