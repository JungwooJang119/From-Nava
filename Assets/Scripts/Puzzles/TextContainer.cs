using UnityEngine;

[CreateAssetMenu(fileName = "TextContainer", menuName = "Text Container")]
public class TextContainer : ScriptableObject {
	public string[] textEntries;
	public int[] reportStart;
	public int[] reportEnd;
}

