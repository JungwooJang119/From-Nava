using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script controls the access to the Lab Report and the events shown On-Screen.

public class LabReport : MonoBehaviour
{
	[SerializeField] private int reportNumber;          // Number of the report, the script will disable if number is invalid;
	[SerializeField] private float range = 2;             // How far can the player be from the terminal to trigger it;
	[SerializeField] private TextContainer reportSO;      // Reference to the Lab Report Text Container, Do Not Change [please];
	[SerializeField] private GameObject textModel;        // Reference to the text prefab to draw on-screen;
	[SerializeField] private GameObject bottomNote;       // Reference to the button press notice mid-report;

	private List<string> _strings2Grab;	// List of strings that will be taken from the ScriptableObject;
	private string _string2Report;		// String that will be grabbed based on the reportNumber;
	private GameObject _playerRef;		// Reference to player object;
	private Transform _player;          // References to player script;
	private bool _playerNear;			// Whether the player is near or not;
	private string _intKey = "z";       // Key used to trigger the interactions;
	private string _spdKey = "x";       // Key used to speed up the text;
	private string _skpKey = "c";		// Key used to skip the writing cinematic;

	private GameObject _tranRef;        // Reference to player object;
	private tranMode _tranScript;       // Reference to Transition script;

	private GameObject _canvasRef;      // Reference to the UI Canvas;
	private RectTransform _canvasRT;	// Reference to the Canvas RectTransform;

	private float _currentDistance;     // Variable to store the distance calculation (object to player);
	private string _state = "Idle";		// State machine var. States: Idle, FadeOut, Writing, Waiting, FadeIn, Inactive;
	private float _timer = 0;           // Timer variable;

	private TextMeshProUGUI _currentText; // Reference to the text instance operating;
	private TextMeshProUGUI _currentNote; // Reference to the bottom note pointing the key to press;

	// Text pacing variables
	private float _letterWait = 0.05f;	// Controls how fast are characters written to the screen;
	private float _normalWait = 0.05f;  // Delay if no button is pressed;
	private int _currentIndex;          // Controls the current limit of the written string;
	private byte _r = 255, _g = 255, _b = 255; // RGB values for the text color;
	private byte _alpha = 255;          // Controls alpha fadeout of text;
	private bool _dotted = false;       // Variable to control the cursor;

	// Alert text variables;
	private float _timer2 = 0;			// Second timer for alpha fading;
	private byte _alpha2 = 0;           // Second alpha for the alert text;
	private bool _alertUp = true;       // State machine to control alert fading;

	// Button Tutorial;
	public GameObject buttonTutorial;   // Reference to button tutorial pop-up;
	private GameObject _tutInstance;    // Reference to instantiate text pop-up;
	private ButtonTutorial _tutScript;	// Reference to instantiated text script;

	// Initialize variable references;
	void Start() {
		_strings2Grab = new List<string>();
		_playerRef = GameObject.Find("Player");
		_tranRef = GameObject.Find("Transition");
		_canvasRef = GameObject.Find("UI Canvas");
		// Shuts down the script to avoid errors if either is missing;
		if (_playerRef == null || _tranRef == null || _canvasRef == null) {
			Debug.Log("Player, Canvas, or Transition Object Not Found. Lab Report Disabled");
		} else {
			_player = _playerRef.GetComponent<Transform>();
			_tranScript = _tranRef.GetComponent<tranMode>();
			_canvasRT = _canvasRef.GetComponent<RectTransform>();
		}
		if (reportNumber >= 0 && reportNumber < reportSO.reportStart.Length) {
			for (int i = reportSO.reportStart[reportNumber]; i < reportSO.reportEnd[reportNumber]; i++) {
				_strings2Grab.Add(reportSO.textEntries[i]);
			}
			FetchString();
		} else {
			Debug.Log("Report Number doesn't exist");
		}
	}

	void Update() {
		// Start Lab Report if player interacts in range \\
		if (((Vector2)_player.position - (Vector2)transform.position).magnitude < range) {
			if (_state == "Idle") {
				if (_tutInstance == null) {
					_tutInstance = Instantiate(buttonTutorial, transform.position, Quaternion.identity);
					_tutScript = _tutInstance.GetComponent<ButtonTutorial>();
					_tutScript.keyToPress = _intKey;
					_tutScript.parent = gameObject;
				} else {
					_tutScript.CancelFade();
				}
				if (Input.GetKeyDown(_intKey)) {
					_state = "FadeOut";
					_timer = _tranScript.DarkenOut() + 1f;
					if (_tutInstance != null) {
						_tutScript.Fade();
					}
				}
			} 
		} else if (_tutInstance && _state == "Idle") {
			_tutScript.Fade();
		}
		// Control how the report is shown after the interaction \\
		if (_timer > 0) {
			_timer -= Time.deltaTime;
		} else if (_state == "FadeOut") {
			_state = "Writing";
			DisplayReport();
		// If the FadeOut is complete, write text \\
		} else if (_state == "Writing" && _currentText != null) {
			if (_currentIndex < _string2Report.Length && _string2Report[_currentIndex] != '\\') {
				_currentText.text = BuildStr(_string2Report, "|");
				_timer = _letterWait;
				_currentIndex++;
			} else if (_currentIndex < _string2Report.Length && _string2Report[_currentIndex] == '\\') {
				_state = "Waiting";
				_currentText.text = BuildStr(_string2Report, "|");
				_timer = 0;
			} else if (_currentIndex == _string2Report.Length) {
				_state = "Waiting";
				_timer = 0;
			}
			if (Input.GetKey(_spdKey)) {
				_letterWait = 0;
			} else {
				_letterWait = _normalWait;
			}
		}
		// If a escape sequence is found, wait for user input \\
		if (_state == "Waiting" && _currentText != null) {
			if (_timer <= 0) {
				if (_dotted) {
					_currentText.text = BuildStr(_string2Report, "|");
					_timer = 0.5f;
					_dotted = false;
				} else {
					_currentText.text = BuildStr(_string2Report, "<color=#00000000>|</color>");
					_timer = 0.5f;
					_dotted = true;
				}
			}
			if (Input.GetKey(_intKey) || Input.GetKey(_spdKey)) {
				if (_currentIndex < _string2Report.Length) {
					_state = "Writing";
					_string2Report = _string2Report.Substring(0,_currentIndex) + "<color=#00000000>|</color>"
									 + _string2Report.Substring(_currentIndex);
					_currentIndex += 31;
					_timer = 0;
				} else if (Input.GetKeyDown(_intKey) || Input.GetKeyDown(_spdKey)) {
					_state = "FadeIn";
				}
			}
		}
		// If the end was reached, fade in \\
		if (_state == "FadeIn" && _currentText != null) {
			if (_strings2Grab.Count <= 0) {
				_tranScript.DarkenIn();
			}
			_alpha -= 5;
			_currentText.color = new Color32(_r, _g, _b, _alpha);
			if (_alpha2 >= 5) { _alpha2 -= 5; };
			_currentNote.color = new Color32(_r, _g, _b, _alpha2);
			if (_alpha < 5) {
				if (FetchString()) {
					DisplayReport();
					_state = "Writing";
					_timer = 0.5f;
					_currentIndex = 0;
					_alpha = 255;
					_currentText.color = new Color32(_r, _g, _b, _alpha);
				} else {
					Destroy(_currentText.gameObject);
					Destroy(_currentNote.gameObject);
					_state = "Inactive";
				}
			}
		}
		// Finish earlier if player presses skip button \\
		if (_state != "Idle" && _state != "FadeIn" && Input.GetKeyDown(_skpKey) && _currentText != null) {
			_state = "Waiting";
			_currentText.text = BuildStr(_string2Report, "<color=#00000000>|</color>");
			_currentIndex = _string2Report.Length;
			_timer = 0;
		}

		// Fading effects for the Button Pop-Up \\
		if (_timer2 > 0) {
			_timer2 -= Time.deltaTime;
		}
		if (_state == "Writing" || _state == "Waiting" || _state == "FadingIn") {
			if (_timer2 <= 0) {
				if (_alertUp) {
					_alpha2++;
					_timer2 = 0.002f;
				}
				else {
					_alpha2--;
					_timer2 = 0.002f;
				}
			}
			if (_alpha2 >= 250) {
				_alertUp = false;
			}
			else if (_alpha2 <= 50) {
				_alertUp = true;
			}
			_currentNote.color = new Color32(_r, _g, _b, _alpha2);
		}
	}
	
	// Instantiates the text and the button/bottom note \\
	// Note: Until the UI Rework, the exact position of the button alert is hardcoded.
    public void DisplayReport() {
		if (_currentText == null) {
			_currentText = Instantiate(textModel, _canvasRef.transform.position, _canvasRef.transform.rotation).GetComponent<TextMeshProUGUI>();
			_currentText.transform.SetParent(_tranRef.transform);
			_currentText.transform.localScale = Vector2.one;
			_currentText.transform.position = new Vector3(_currentText.transform.position.x, _currentText.transform.position.y + 0.75f, _currentText.transform.position.z);
		}
		_currentText.text = "<color=#00000000>" + _string2Report + "</color>";
		if (_currentNote == null) {
			_currentNote = Instantiate(bottomNote, _canvasRef.transform.position, _canvasRef.transform.rotation).GetComponent<TextMeshProUGUI>();
			_currentNote.transform.SetParent(_tranRef.transform);
			_currentNote.transform.localScale = Vector2.one;
			_currentNote.transform.position = new Vector3(_currentNote.transform.position.x, _currentNote.transform.position.y - 7, _currentNote.transform.position.z);
			_currentNote.text = "Speed [" + _spdKey.ToUpper() + "]        Advance [" + _intKey.ToUpper() + "]        Skip [" + _skpKey.ToUpper() + "]";
		}
	}

	// Builds the string as if it were typed in real-time.
	private string BuildStr(string str, string lineAppend) {
		string nstr = (str.Substring(0, _currentIndex) + lineAppend
									+ "<color=#00000000>" + str.Substring(_currentIndex) + "</color>")
									.Replace("\\n", "\n"); // Fixing a weird string parsing issue with Unity;
		return nstr;
	}

	// Determines whether there are more pages to display, and if so changes the current string;
	private bool FetchString() {
		if (_strings2Grab.Count > 0) {
			_string2Report = _strings2Grab[0];
			_strings2Grab.RemoveAt(0);
			return true;
		}
		return false;
	}
}