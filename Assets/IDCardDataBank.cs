using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDCardDataBank : MonoBehaviour
{
    [Serializable]
	private struct IDCardInfo {
		public IDCardType room;
		public IDCardData data;
	} [SerializeField] private IDCardInfo[] idCards;

	public enum IDCardType {
		A1, A2, A3, B1, B2, B3, C1, C2, C3
    } 

	private Dictionary<IDCardType, IDCardData> idCardDict;

	// Initialize IDCard Dictionary;
	void Start() {
		idCardDict = new Dictionary<IDCardType, IDCardData>();
		foreach (IDCardInfo idCard in idCards) {
			idCardDict[idCard.room] = idCard.data;
		}
	}

	public IDCardData GetIDCardData(string room) {
		IDCardType idCardType = 0;
		Enum.TryParse(room, out idCardType);
		return idCardDict[idCardType];
	}

	public Dictionary<IDCardType, IDCardData> GetIDCardDict() {
		return idCardDict;
    }
}
