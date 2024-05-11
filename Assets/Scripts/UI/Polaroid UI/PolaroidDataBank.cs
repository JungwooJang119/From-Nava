using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class PolaroidDataBank : MonoBehaviour {

	[Serializable]
	private struct PolaroidInfo {
		public PolaroidType room;
		public PolaroidData data;
	} [SerializeField] private PolaroidInfo[] polaroids;

	public enum PolaroidType {
		A1, A2, A3, B1, B2, B3, C1, C2, C3
    }

	private Dictionary<PolaroidType, PolaroidData> polaroidDict;

	void Start() {
		polaroidDict = new Dictionary<PolaroidType, PolaroidData>();
		foreach (PolaroidInfo polaroid in polaroids) {
			polaroidDict[polaroid.room] = polaroid.data;
		}
	}

	public PolaroidData GetPolaroidData(string room) {
		PolaroidType polaroidType = 0;
		Enum.TryParse(room, out polaroidType);
		return polaroidDict[polaroidType];
	}

	public Dictionary<PolaroidType, PolaroidData> GetPolaroidDict() {
		return polaroidDict;
    }
}*/