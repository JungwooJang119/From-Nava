using UnityEngine;

[CreateAssetMenu(fileName = "TextContainer", menuName = "Text Container")]
public class TextContainer : ScriptableObject {
	[Tooltip("Each element of this list is a page of the Lab Report.\n" +
			 "Use the '\\n\\n' escape sequence to add a break.")]
	public string[] textEntries;
}

