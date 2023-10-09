using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EndingLogic : MonoBehaviour {

	[SerializeField] private tranMode transitionManager;
	[SerializeField] private Image background;
	[SerializeField] private float initialFrameWait;
	[SerializeField] private float finalFadeTime;
	[SerializeField] private int levelTransitionIndex;
	[SerializeField] private EndingLogicData data;
	[SerializeField] private string initialClipName;

	public EndingLogicData Data => data;

	private List<EndingElement> tranElements;

	private bool await;
	private bool transitioning = true;

	void Start() {
		tranElements = new List<EndingElement>(data.tranElements);
		StartCoroutine(Intro());
	}

    void Update() {
		if (!transitioning) {
			if (tranElements.Count > 0) StartCoroutine(Transition(tranElements[0]));
			else StartCoroutine(FinalFade(finalFadeTime));
		}
    }

	private IEnumerator FinalFade(float finalFadeTime) {
		yield return new WaitForSeconds(transitionManager.FadeOut(finalFadeTime));
		UnityEngine.SceneManagement.SceneManager.LoadScene(levelTransitionIndex);
	}

	private IEnumerator Intro() {
		yield return new WaitForSeconds(1);
		if (!string.IsNullOrWhiteSpace(initialClipName)) AudioControl.Instance.PlayMusic(initialClipName, false);
		yield return new WaitForSeconds(initialFrameWait);
		transitioning = false;
    }

	private IEnumerator Transition(EndingElement element) {
		transitioning = true;
		switch (element.GetType().Name) {
			case nameof(EndingTransition):
				EndingTransition et = element as EndingTransition;
				transitionManager.SetTransitionColor(et.transitionColor);
				TriggerAudio(et.onUpAudio);
				yield return new WaitForSeconds(transitionManager.FadeOut(et.transitionUpLength));
				
				background.sprite = et.backgroundSprite;
				TriggerAudio(et.onIntermezzoAudio);
				yield return new WaitForSeconds(et.intermezzoWait);

				TriggerAudio(et.onDownAudio);
				yield return new WaitForSeconds(transitionManager.FadeIn(et.transitionDownLength));
				TriggerAudio(et.postTransitionAudio);
				transitionManager.SetTransitionColor(Color.black);
				yield return new WaitForSeconds(et.postTransitionWait);
				break;
				
			case nameof(AudioTrigger):
				TriggerAudio(element as AudioTrigger);
				break;
			case nameof(PolaroidInteraction):
				PolaroidInteraction pi = element as PolaroidInteraction;
				if (pi.tutorialItem == null) pi.tutorialItem = FindObjectOfType<EndingPolaroid>(true);
				await = true;
				pi.tutorialItem.gameObject.SetActive(true);
				pi.tutorialItem.OnScriptEnd += TriggerEnd;
				while (await) yield return null;
				break;
        } tranElements.RemoveAt(0);
		transitioning = false;
		yield return null;
    }

	private void TriggerEnd() {
		tranElements.RemoveAt(0);
		transitioning = false;
		await = false;
	}

	private void TriggerAudio(AudioTrigger at) {
		if (!at.use) return;
		if (at.sfx) AudioControl.Instance.PlayVoidSFX(at.clipName);
		else AudioControl.Instance.PlayMusic(at.clipName, false);
	}
}

#if UNITY_EDITOR

[CustomEditor(typeof(EndingLogic))]
public class EndingLogicEditor : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		EditorGUILayout.Separator();

		EndingLogic logic = target as EndingLogic;

		GUIStyle windowBox = new GUIStyle(GUI.skin.window) {
			padding = new RectOffset(5, 5, 5, 5),
			stretchWidth = false,
			stretchHeight = false,
		};

		if (logic.Data == null) return;

		using (var scope = new EditorGUI.ChangeCheckScope()) {
			if (logic.Data.tranElements == null) logic.Data.tranElements = new List<EndingElement>();
			using (new EditorGUILayout.VerticalScope(windowBox)) {
				for (int i = 0; i < logic.Data.tranElements.Count; i++) {
					EndingElement element = logic.Data.tranElements[i];
					using (new EditorGUILayout.HorizontalScope(windowBox)) {
						using (new EditorGUILayout.VerticalScope(windowBox)) {
							switch (element.GetType().Name) {
								case nameof(EndingTransition):
									EndingTransition et = element as EndingTransition;
								
									et.backgroundSprite = EditorGUILayout.ObjectField(et.backgroundSprite, typeof(Sprite), true) as Sprite;
									et.transitionColor = EditorGUILayout.ColorField(et.transitionColor);

									using (new EditorGUILayout.HorizontalScope(windowBox)) {
										GUILayout.Label("Transition Time", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
									} et.transitionUpLength = EditorGUILayout.FloatField("Up:", et.transitionUpLength);
									et.intermezzoWait = EditorGUILayout.FloatField("Mid:", et.intermezzoWait);
									et.transitionDownLength = EditorGUILayout.FloatField("Down:", et.transitionDownLength);
									et.postTransitionWait = EditorGUILayout.FloatField("End:", et.postTransitionWait);

									using (new EditorGUILayout.HorizontalScope(windowBox)) {
										GUILayout.Label("Audio", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
									} et.onUpAudio = AudioTriggerField("Up:", et.onUpAudio);
									et.onIntermezzoAudio = AudioTriggerField("Mid:", et.onIntermezzoAudio);
									et.onDownAudio = AudioTriggerField("Down:", et.onDownAudio);
									et.postTransitionAudio = AudioTriggerField("End:", et.postTransitionAudio);

									break;
								case nameof(AudioTrigger):
									AudioTrigger at = element as AudioTrigger;
									at = AudioTriggerField("Cue: ", at);
									break;
								case nameof(PolaroidInteraction):
									PolaroidInteraction pi = element as PolaroidInteraction;
									pi.tutorialItem = EditorGUILayout.ObjectField(pi.tutorialItem, typeof(EndingPolaroid), true) as EndingPolaroid;
									break;
							}
						} if (GUILayout.Button((Texture2D) EditorGUIUtility.IconContent("TreeEditor.Trash").image, GUILayout.ExpandHeight(true))) {
							logic.Data.tranElements.RemoveAt(i);
						}
					} using (new EditorGUILayout.HorizontalScope()) {
						if (GUILayout.Button("Add Transition")) logic.Data.tranElements.Insert(i + 1, new EndingTransition());
						if (GUILayout.Button("Add Audio Trigger")) logic.Data.tranElements.Insert(i + 1, new AudioTrigger());
						if (GUILayout.Button("Add Polaroid Interaction")) logic.Data.tranElements.Insert(i + 1, new PolaroidInteraction());
					} if (i < logic.Data.tranElements.Count - 1) { EditorGUILayout.Separator(); EditorGUILayout.Separator(); }
				} if (logic.Data.tranElements.Count == 0) {
					using (new EditorGUILayout.VerticalScope()) {
						if (GUILayout.Button("Add Transition")) logic.Data.tranElements.Add(new EndingTransition());
						if (GUILayout.Button("Add Audio Trigger")) logic.Data.tranElements.Add(new AudioTrigger());
						if (GUILayout.Button("Add Polaroid Interaction")) logic.Data.tranElements.Add(new PolaroidInteraction());
					}
				}
			} if (scope.changed) {
				EditorUtility.SetDirty(logic.Data);
			}
		}
	}

	private AudioTrigger AudioTriggerField(string label, AudioTrigger at) {
		if (at == null) at = new AudioTrigger();

		using (new EditorGUILayout.HorizontalScope()) {
			GUILayout.Label(label, GUILayout.Width(42));
			at.use = EditorGUILayout.Toggle(at.use, GUILayout.Width(20)); GUILayout.Label("Use"); EditorGUILayout.Separator();
			if (!at.use) GUI.enabled = false;
			at.sfx = EditorGUILayout.Toggle(at.sfx, GUILayout.Width(20)); GUILayout.Label("SFX"); EditorGUILayout.Separator();
			GUILayout.Label("Clip Name"); at.clipName = EditorGUILayout.TextField(at.clipName);
		} GUI.enabled = true;
		return at;
	}
}

#endif