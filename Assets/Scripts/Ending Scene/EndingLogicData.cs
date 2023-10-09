using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndingLogicData : ScriptableObject {
	[SerializeReference] public List<EndingElement> tranElements;
}

[System.Serializable]
public abstract class EndingElement { }

[System.Serializable]
public class EndingTransition : EndingElement {
	public Sprite backgroundSprite;

	public float transitionUpLength;
	public float intermezzoWait;
	public float transitionDownLength;
	public float postTransitionWait;

	public Color transitionColor;

	public AudioTrigger onUpAudio;
	public AudioTrigger onIntermezzoAudio;
	public AudioTrigger onDownAudio;
	public AudioTrigger postTransitionAudio;
}

[System.Serializable]
public class AudioTrigger : EndingElement {
	public bool use;
	public bool sfx;
	public string clipName;
}

[System.Serializable]
public class PolaroidInteraction : EndingElement {
	public EndingPolaroid tutorialItem;
}
