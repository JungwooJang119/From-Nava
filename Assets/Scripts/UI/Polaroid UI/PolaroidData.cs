using System;
using UnityEditor;
using UnityEngine;

// Scriptable Object for polaroid data. This script might read like a mess,
// but the idea is to make the inspector view a bit cleaner and comprehensive :D

[CreateAssetMenu(fileName = "PolaroidData", menuName = "Polaroid Data")]
public class PolaroidData : ScriptableObject {
	[Tooltip("Sprite associated with the polaroid.")]
	public Sprite sprite;

	[TextAreaAttribute]
	[Tooltip("Text displayed beneath the polaroid.")]
	public string text;
}
