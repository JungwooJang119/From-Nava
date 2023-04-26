using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Tutorial Data")]
public class TutorialData : ScriptableObject {
	[Tooltip("Header line for the first time the tutorial is shown.")]
	public string textHeaderNew;

	[Tooltip("Header line for any time after the first one.")]
	public string textHeader;
	
	[TextAreaAttribute]
	[Tooltip("Text that populates the screen between the header and the visuals.")]
	public string text;

	[Tooltip("Image showcasing the input required to use the showcased mechanic.")]
	public Sprite inputImage;

	[Tooltip("Video to be shown during the tutorial")]
	public VideoClip videoClip;
}
