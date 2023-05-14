using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportManager : MonoBehaviour, ICollectibleManager
{
	public event Action OnReportEnd;

	private ReportTextBank reportTextBank;
	private TextContainer reportSO;

	private GameObject reportText;                        // Reference to the text prefab to draw on-screen;
	private GameObject bottomNote;                        // Reference to the button press notice mid-report;

	private List<string> strings2Grab;          // List of strings that will be taken from the ScriptableObject;
	private string string2Report;               // String that will be grabbed based on the reportNumber;

	private string intKey = "space";                // Key used to trigger interactions;
	private bool waitForPress = false;              // Lazy boolean, zero creativity atm ;-;

	// References;
	private Transform canvasTransform;
	private tranMode transitionScript;

	// Now a real state machine! \0/
	private enum State {
		Idle,
		Start,
		Writing,
		Waiting,
		End,
	} private State state = State.Idle;

	private float textTimer = 0;

	private TextMeshProUGUI currentText; // Reference to the text instance operating;
	private TextMeshProUGUI currentNote; // Reference to the bottom note pointing the key to press;

	// Text pacing variables
	private float letterWait = 0.05f;  // Controls how fast are characters written to the screen;
	private int currentIndex;          // Controls the current limit of the written string;
	private bool dotted = false;       // Variable to control the cursor;

	// Text color and opacity;
	private byte _r = 255, _g = 255, _b = 255; // RGB values for the text color;
	private float textAlpha = 1;              // Controls alpha fadeout of text;
	private float noteAlpha = 0;                // Second alpha for the bottom alert text;
	private bool alertUp;

	private string[] soundStrings = { "Report Key v1", "Report Key v2" };

	void Start() {
		reportTextBank = GetComponent<ReportTextBank>();
		strings2Grab = new List<string>();

		foreach (Transform t in transform) {
			if (t.gameObject.name == "ReportText") reportText = t.gameObject;
			if (t.gameObject.name == "BottomNote") bottomNote = t.gameObject;
		}

		transitionScript = ReferenceSingleton.Instance.transition;
		canvasTransform = transitionScript.transform.parent;
	}

	void Update() {
		switch (state) {
			case State.Start:
				if (textTimer <= 0) {
					state = State.Writing;
				} break;

			case State.Writing:
				if (currentText != null) {
					if (textTimer <= 0) {
						// Move the start of the invisible color tag toward the end, thus showing more characters;
						if (currentIndex < string2Report.Length && string2Report[currentIndex] != '\\') {
							AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0, 2)], 0.15f);
							currentText.text = BuildStr(string2Report, "|");
							textTimer = letterWait;
							currentIndex++;
						} else if (currentIndex < string2Report.Length && string2Report[currentIndex] == '\\') {
							state = State.Waiting;
							currentText.text = BuildStr(string2Report, "|");
							textTimer = 0;
						} else if (currentIndex == string2Report.Length) {
							state = State.Waiting;
							textTimer = 0;
						}
					}
					if (Input.GetKeyDown(intKey)) {
						state = State.Waiting;
						AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0, 2)], 0.2f);
						currentText.text = BuildStr(string2Report, "<color=#00000000>|</color>");
						currentIndex = string2Report.Length;
						waitForPress = true;
						textTimer = 0;
					}
				} break;

			case State.Waiting:
				if (currentText != null) {
					// Intermitent flashing of the pointer character;
					if (textTimer <= 0) {
						if (dotted) {
							currentText.text = BuildStr(string2Report, "|");
							textTimer = 0.5f;
							dotted = false;
						} else {
							currentText.text = BuildStr(string2Report, "<color=#00000000>|</color>");
							textTimer = 0.5f;
							dotted = true;
						}
					}
					// Continue writing or finish report if an input key is pressed;
					if (Input.GetKeyUp(intKey)) {
						if (currentIndex < string2Report.Length) {
							state = State.Writing;
							string2Report = string2Report.Substring(0, currentIndex) + "<color=#00000000>|</color>"
											 + string2Report.Substring(currentIndex);
							currentIndex += 31; // Skip length of tag + length of escape sequence;
						}
						else if (!waitForPress) {
							state = State.End;
						}
						else {
							waitForPress = false;
						}
						textTimer = 0;
					}
				} break;

			case State.End:
				if (currentText != null) {
					// Fade out all text;
					var rate = Time.deltaTime * 4f;
					textAlpha = Mathf.Max(0, textAlpha - rate);
					currentText.color = new Color(_r, _g, _b, textAlpha);
					if (noteAlpha >= 0) { noteAlpha = Mathf.Max(0, textAlpha - rate); };
					currentNote.color = new Color(_r, _g, _b, noteAlpha);
					// Continue to next page if there's one or finalize report;
					if (textAlpha == 0) {
						if (FetchString()) {
							NextPage();
							state = State.Writing;
							textTimer = 0.5f;
							currentIndex = 0;
							textAlpha = 1;
							currentText.color = new Color(_r, _g, _b, textAlpha);
						} else {
							state = State.Idle;
							ResetElements();
							OnReportEnd?.Invoke();
						}
					}
				} break;
		}

		// Fading effect of the pop-up;
		if (state == State.Writing || state == State.Waiting) {
			if (noteAlpha >= 1) {
				alertUp = false;
			} else if (noteAlpha <= 0.1f) {
				alertUp = true;
			}
			if (alertUp) {
				noteAlpha += Time.deltaTime;
			} else {
				noteAlpha -= Time.deltaTime;
			} currentNote.color = new Color(_r, _g, _b, noteAlpha);
		}

		if (textTimer > 0) textTimer -= Time.deltaTime;
	}		
	
	// Instantiates the text and the button/bottom note \\
	// Note: Due to time constraints, the text does not scale well with the UI. Lmk if you are interested in that. -Carlos;
    public void Display(string name, float transitionTime) {
		reportSO = reportTextBank.GetReportData(name);
		strings2Grab.Clear();
		foreach (string report in reportSO.textEntries) {
			strings2Grab.Add(report);
		} FetchString();

		if (currentText == null) {
			reportText.SetActive(true);
			var reportTextTransform = reportText.transform;
			reportTextTransform.SetParent(canvasTransform);
			reportTextTransform.localScale = Vector3.one;
			reportTextTransform.rotation = canvasTransform.rotation;
			reportTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 0.75f, canvasTransform.position.z);
			currentText = reportText.GetComponent<TextMeshProUGUI>();
		} currentText.text = "<color=#00000000>" + string2Report + "</color>";

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

		textTimer = transitionTime;
		state = State.Start;
		textAlpha = 1;
		noteAlpha = 0;
		currentText.color = new Color(_r, _g, _b, textAlpha);
		currentNote.color = new Color(_r, _g, _b, noteAlpha);
	}

	private void NextPage() {
		currentText.text = "<color=#00000000>" + string2Report + "</color>";
	}

	// Builds the string as if it were typed in real-time.
	private string BuildStr(string str, string lineAppend) {
		string nstr = (str.Substring(0, currentIndex) + lineAppend
									+ "<color=#00000000>" + str.Substring(currentIndex) + "</color>")
									.Replace("\\n", "\n"); // Fixing a weird string parsing issue with Unity;
		return nstr;
	}

	// Determines whether there are more pages to display, and if so changes the current string;
	private bool FetchString() {
		if (strings2Grab.Count > 0) {
			string2Report = strings2Grab[0];
			strings2Grab.RemoveAt(0);
			return true;
		} return false;
	}

	private void ResetElements() {
		currentIndex = 0;

		reportText.transform.SetParent(transform);
		reportText.SetActive(false);
		currentText = null;

		bottomNote.transform.SetParent(transform);
		bottomNote.SetActive(false);
		currentNote = null;
	}
}