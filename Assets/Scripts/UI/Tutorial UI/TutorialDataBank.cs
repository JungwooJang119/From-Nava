using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDataBank : MonoBehaviour {
	[Serializable]
	private struct TutorialInfo {
		public string name;
		public TutorialData data;
	}
	[SerializeField] private TutorialInfo[] tutorials;

	private Dictionary<string, TutorialData> tutorialDict;

	// Initialize Polaroid Dictionary;
	void Start() {
		tutorialDict = new Dictionary<string, TutorialData>();
		foreach (TutorialInfo tutorial in tutorials) {
			tutorialDict[tutorial.name] = tutorial.data;
		}
	}

	public TutorialData GetTutorialData(string name) {
		return tutorialDict[name];
	}
}