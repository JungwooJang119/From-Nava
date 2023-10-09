using System;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "SideRoomKeyData", menuName = "Side Room Key Data")]
public class SideRoomKeyData : ScriptableObject
{
    [Tooltip("Sprite associated with the side room key.")]
	public Sprite sprite;

	[TextAreaAttribute]
	[Tooltip("Text displayed beneath the side room key.")]
	public string text;
}
