using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script controls the access to the Lab Report and the events shown On-Screen.

public class LabReport : MonoBehaviour
{
	[SerializeField] private int reportNumber;			  // Number of the report, the script will disable if number is invalid;
	[SerializeField] private float range = 2;             // How far can the player be from the terminal to trigger it;
	[SerializeField] private TextContainer reportSO;      // Reference to the Lab Report Text Container, Do Not Change [please];

	private GameObject reportText;						  // Reference to the text prefab to draw on-screen;
	private GameObject bottomNote;						  // Reference to the button press notice mid-report;

	private List<string> strings2Grab;			// List of strings that will be taken from the ScriptableObject;
	private string string2Report;				// String that will be grabbed based on the reportNumber;

	private string intKey = "space";				// Key used to trigger interactions;
	private bool waitForPress = false;				// Lazy boolean, zero creativity atm ;-;

	// References;
	private PlayerController playerController;
	private Transform playerTransform;
	private Transform transitionTransform;
	private Transform canvasTransform;
	private tranMode transitionScript;

	// Now a real state machine! \0/
	private enum State {
		Idle,
		Start,
		Writing,
		Waiting,
		End,
		Done,
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
	private byte textAlpha = 255;			   // Controls alpha fadeout of text;
	private byte noteAlpha = 0;                // Second alpha for the bottom alert text;
	private bool alertUp;

	// Button Tutorial;
	[SerializeField] private GameObject buttonTutorial; // Reference to button tutorial pop-up;
	private GameObject tutInstance;		// Reference to instantiate text pop-up;
	private ButtonTutorial tutScript;   // Reference to instantiated text script;

	private string[] soundStrings = { "Report Key v1", "Report Key v2" };

	// Room Control;
	public event Action OnReportRead;

	// Initialize variable references;
	void Start() {
		// Find children;
		foreach (Transform t in transform) {
			if (t.gameObject.name == "ReportText") reportText = t.gameObject;
			if (t.gameObject.name == "BottomNote") bottomNote = t.gameObject;
		}
		// Fetch strings from the SO;
		strings2Grab = new List<string>();
		if (reportNumber >= 0 && reportNumber < reportSO.reportStart.Length) {
			for (int i = reportSO.reportStart[reportNumber]; i < reportSO.reportEnd[reportNumber]; i++) {
				strings2Grab.Add(reportSO.textEntries[i]);
			}
			FetchString();
		} else { Debug.LogWarning("Report Number doesn't exist"); }

		// Fetch references and throw an error if one isn't found;
		playerController = PlayerController.Instance;
		var tranRef = GameObject.Find("Transition");
		var canvasRef = GameObject.Find("UI Canvas");
		if (playerController == null || tranRef == null || canvasRef == null) {
			Debug.LogWarning("Player, Canvas, or Transition Object Not Found. Lab Report Disabled");
		} else {
			playerTransform = playerController.gameObject.transform;
			transitionTransform = tranRef.transform;
			canvasTransform = canvasRef.transform;
			transitionScript = tranRef.GetComponent<tranMode>();
		}
	}

	void Update() {
		switch (state) {
			case State.Idle:
				if (((Vector2)playerTransform.position - (Vector2)transform.position).magnitude < range) {
					if (tutInstance == null) {
						tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
						tutScript = tutInstance.GetComponent<ButtonTutorial>();
						tutScript.SetUp(intKey, gameObject);
					} else {
						tutScript.CancelFade();
					}
					// Start the Lab Report;
					if (Input.GetKeyDown(intKey)) {
						playerController.DeactivateMovement();
						state = State.Start;
						textTimer = transitionScript.DarkenOut() + 0.5f;
						if (tutInstance != null) {
							tutScript.Fade();
						}
					}
				} else if (tutInstance) {
					tutScript.Fade();
				}
				break;
			case State.Start:
				if (textTimer < 0) {
					state = State.Writing;
					DisplayReport();
				} break;

			case State.Writing:
				if (textTimer <= 0 && currentText != null) {
					// Move the start of the invisible color tag toward the end, thus showing more characters;
					if (currentIndex < string2Report.Length && string2Report[currentIndex] != '\\') {
						AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0,2)], 0.15f);
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
						} else if (!waitForPress) {
							state = State.End;
						} else {
							waitForPress = false;
						}
						textTimer = 0;
					}
				} break;

			case State.End:
				if (currentText != null) {
					// Finish report if there are no more pages to present;
					if (strings2Grab.Count == 0) transitionScript.DarkenIn();
					// Fade out all text;
					textAlpha = (byte) Mathf.Max(0, (int) textAlpha - 10);
					currentText.color = new Color32(_r, _g, _b, textAlpha);
					if (noteAlpha >= 5) { noteAlpha = (byte) Mathf.Max(0, (int) textAlpha - 10); };
					currentNote.color = new Color32(_r, _g, _b, noteAlpha);
					// Continue to next page if there's one or finalize report;
					if (textAlpha < 5) {
						if (FetchString()) {
							DisplayReport();
							state = State.Writing;
							textTimer = 0.5f;
							currentIndex = 0;
							textAlpha = 255;
							currentText.color = new Color32(_r, _g, _b, textAlpha);
						} else {
							Destroy(currentText.gameObject);
							Destroy(currentNote.gameObject);
							playerController.ActivateMovement();
							state = State.Done;
							OnReportRead?.Invoke();
						}
					}
				} break;
		}

		// Finish earlier if player presses skip button;
		if (state == State.Writing && Input.GetKeyDown(intKey) && currentText != null) {
			state = State.Waiting;
			AudioControl.Instance.PlayVoidSFX(soundStrings[UnityEngine.Random.Range(0, 2)], 0.2f);
			currentText.text = BuildStr(string2Report, "<color=#00000000>|</color>");
			currentIndex = string2Report.Length;
			waitForPress = true;
			textTimer = 0;
		}

		// Fading effect of the pop-up;
		if (ReportIsActive()) {
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
		}

		if (textTimer > 0) textTimer -= Time.deltaTime;
	}
	
	// Instantiates the text and the button/bottom note \\
	// Note: Due to time constraints, the text does not scale well with the UI. Lmk if you are interested in that. -Carlos;
    private void DisplayReport() {
		if (currentText == null) {
			reportText.SetActive(true);
			var reportTextTransform = reportText.transform;
			reportTextTransform.SetParent(canvasTransform);
			reportTextTransform.localScale = Vector2.one;
			reportTextTransform.rotation = canvasTransform.rotation;
			reportTextTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y + 0.75f, canvasTransform.position.z);
			currentText = reportText.GetComponent<TextMeshProUGUI>();
		} currentText.text = "<color=#00000000>" + string2Report + "</color>";

		if (currentNote == null) {
			bottomNote.SetActive(true);
			var bottomNoteTransform = bottomNote.transform;
			bottomNoteTransform.SetParent(canvasTransform);
			bottomNoteTransform.localScale = Vector2.one;
			bottomNoteTransform.rotation = canvasTransform.rotation;
			bottomNoteTransform.position = new Vector3(canvasTransform.position.x, canvasTransform.position.y - 7, canvasTransform.position.z);
			currentNote = bottomNote.GetComponent<TextMeshProUGUI>();
			currentNote.text = "Advance [" + intKey.ToUpper() + "]";
		}
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

	private bool ReportIsActive() {
		return state != State.Idle && state != State.Start && state != State.Done;
	}
}