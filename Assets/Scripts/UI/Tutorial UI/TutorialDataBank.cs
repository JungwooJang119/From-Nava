using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDataBank : MonoBehaviour {
	
	public enum TutorialType {
		Basics,
		Melee,
		Fireball,
		Iceball,
		Chair,
		Windblast,
    }

	[Serializable]
	private struct TutorialInfo {
		public TutorialType type;
		public TutorialData data;
	}
	[SerializeField] private TutorialInfo[] tutorials;

	private Dictionary<TutorialType, TutorialData> tutorialDict;

	// Initialize Polaroid Dictionary;
	void Start() {
		tutorialDict = new Dictionary<TutorialType, TutorialData>();
		foreach (TutorialInfo tutorial in tutorials) {
			tutorialDict[tutorial.type] = tutorial.data;
		}
	}

	public TutorialData GetTutorialData(string name) {
		var type = TutorialType.Basics;
		Enum.TryParse(name, out type);
		return tutorialDict[type];
	}

	public Dictionary<TutorialType, TutorialData> GetTutorialDict() {
		return tutorialDict;
    }
}