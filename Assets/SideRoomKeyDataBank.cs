using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideRoomKeyDataBank : MonoBehaviour
{
    [Serializable]
	private struct SideRoomKeyInfo {
		public SideRoomKeyType room;
		public SideRoomKeyData data;
	} [SerializeField] private SideRoomKeyInfo[] sideRoomKeys;

	public enum SideRoomKeyType {
		A1, A2, A3, B1, B2, B3, C1, C2, C3
    } 

	private Dictionary<SideRoomKeyType, SideRoomKeyData> sideRoomKeyDict;

	// Initialize Polaroid Dictionary;
	void Start() {
		sideRoomKeyDict = new Dictionary<SideRoomKeyType, SideRoomKeyData>();
		foreach (SideRoomKeyInfo sideRoomKey in sideRoomKeys) {
			sideRoomKeyDict[sideRoomKey.room] = sideRoomKey.data;
		}
	}

	public SideRoomKeyData GetSideRoomKeyData(string room) {
		SideRoomKeyType sideRoomKeyType = 0;
		Enum.TryParse(room, out sideRoomKeyType);
		return sideRoomKeyDict[sideRoomKeyType];
	}

	public Dictionary<SideRoomKeyType, SideRoomKeyData> GetSideRoomKeyDict() {
		return sideRoomKeyDict;
    }
}
