using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidDataBank : MonoBehaviour
{
	[Serializable]
	private struct PolaroidInfo {
		public string room;
		public PolaroidData data;
	}
	[SerializeField] private PolaroidInfo[] polaroids;

	private Dictionary<string, PolaroidData> polaroidDict;

	// Initialize Polaroid Dictionary;
	void Start() {
		polaroidDict = new Dictionary<string, PolaroidData>();
		foreach (PolaroidInfo polaroid in polaroids) {
			polaroidDict[polaroid.room] = polaroid.data;
		}
	}

	public PolaroidData GetPolaroidData(string room) {
		return polaroidDict[room];
	}
}
