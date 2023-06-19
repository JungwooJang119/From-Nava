using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A singleton to hold references to commonly used managers and elements that are not static,
// and avoid using GameObject.Find() >.>

public class ReferenceSingleton : Singleton<ReferenceSingleton>
{
	public GameObject mainCamera;
	public tranMode transition;
	public RoomLights roomLights;

	[Header("Collectible References (separated by commas):")]
	[Header("  Polaroids (Room Names):")]
	[Header("   - A1, A2, A3, B1, B2, B3, C1, C2, C3")]
	[Header("  Tutorials (Spell Names):")]
	[Header("   - Melee, Fireball, Iceball, Chair, Windblast")]
	[Header("  Reports:")]
	[Header("   - Ice Tower, Greene, Log 12, Letter")]
	[Space(10)]
	public CollectibleController collectibleController;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}
}
