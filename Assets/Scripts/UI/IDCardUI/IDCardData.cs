using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IDCardData", menuName = "ID Card Data")]
public class IDCardData : ScriptableObject
{
    [Tooltip("Sprite associated with the ID Card.")]
	public Sprite sprite;

	[TextAreaAttribute]
	[Tooltip("Text displayed beneath the ID card.")]
	public string text;
}
