using UnityEngine;

[CreateAssetMenu(fileName = "Report Data", menuName = "Item Data/Report Data")]
public class ReportData : ScriptableItem {
	[Tooltip("Each element of this list is a page of the Lab Report.\n" +
			 "Use the '\\n\\n' escape sequence to add a break.")]
	public string[] textEntries;
}