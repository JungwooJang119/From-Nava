using System;
using System.Collections;
using System.Collections.Generic;
using static NotificationManager;
using UnityEngine;

public class CollectibleController : MonoBehaviour {
	// Responsibilites. Receive calls from objects and initialize Lab Reports, Tutorials, and Polaroid showcases.
	// Responsible for fading in and out of the showcases and communicating success to the caller.

	public event Action OnCallsEnd;

	public enum CollectibleType {
		Polaroid,
		Tutorial,
		Report,
	}

	private PolaroidManager polaroidManager;
	private TutorialManager tutorialManager;
	private ReportManager reportManager;
	private NotificationManager notificationManager;
	private NotificationType notificationType = NotificationType.None;

	private tranMode transition;

	private struct Call {
		public CollectibleType type;
		public string name;
		public bool firstTime;
		public Call(CollectibleType type, string name, bool firstTime) {
			this.type = type;
			this.name = name;
			this.firstTime = firstTime;
		}
	} private List<Call> callStack;

	private List<string> polaroidsClaimed;
	private List<string> tutorialsClaimed;

	private bool busy;

	// Start is called before the first frame update
	void Start() {
		callStack = new List<Call>();
		polaroidsClaimed = new List<string>();
		tutorialsClaimed = new List<string>();

		polaroidManager = GetComponentInChildren<PolaroidManager>();
		polaroidManager.OnDisplayEnd += CollectibleManager_OnDisplayEnd;
		tutorialManager = GetComponentInChildren<TutorialManager>();
		tutorialManager.OnTutorialEnd += CollectibleManager_OnDisplayEnd;
		reportManager = GetComponentInChildren<ReportManager>();
		reportManager.OnReportEnd += CollectibleManager_OnDisplayEnd;
		notificationManager = GetComponentInChildren<NotificationManager>();

		transition = transform.parent.GetComponentInChildren<tranMode>();
	}

	// Update is called once per frame
	void Update() {
		if (callStack.Count > 0 && !busy) {
			switch (callStack[0].type) {
				case CollectibleType.Polaroid:
					DisplayCollectible(polaroidManager, callStack[0].name, callStack[0].firstTime);
					break;
				case CollectibleType.Tutorial:
					DisplayCollectible(tutorialManager, callStack[0].name, callStack[0].firstTime);
					break;
				case CollectibleType.Report:
					DisplayCollectible(reportManager, callStack[0].name, callStack[0].firstTime);
					break;
			}
			callStack.RemoveAt(0);
			busy = true;
		}
	}

	private void DisplayCollectible<T>(T manager, string name, bool firstTime = true) where T : ICollectibleManager {
		var transitionTime = transition.DarkenOut();
		if (firstTime) {
			manager.Display(name, transitionTime);
		} else {
			manager.Display(name);
		}
	}

	public void AddCall(CollectibleType type, string name, bool firstTime = true) {
		if (firstTime) {
			if (type == CollectibleType.Polaroid) {
				if (polaroidsClaimed.Contains(name)) {
					notificationManager.Notify(NotificationType.CollectibleRedundant);
					return;
				} else {
					notificationType = NotificationType.PolaroidClaimed;
					polaroidsClaimed.Add(name);
				}
			} else if (type == CollectibleType.Tutorial) {
				if (tutorialsClaimed.Contains(name)) {
					return;
				} else {
					tutorialsClaimed.Add(name);
				}
			}
		}
		//if (type == CollectibleType.Polaroid && firstTime) AudioControl.Instance.FadeMusic();
		callStack.Add(new Call(type, name, firstTime));
	}

	public bool CheckClaimedStatus(CollectibleType type, string name) {
		switch (type) {
			case CollectibleType.Polaroid:
				return polaroidsClaimed.Contains(name);
			case CollectibleType.Tutorial:
				return tutorialsClaimed.Contains(name);
			default:
				return false;
		}
	}

	private void CollectibleManager_OnDisplayEnd() {
		if (callStack.Count == 0) {
			OnCallsEnd?.Invoke();
			transition.DarkenIn();
			notificationManager.Notify(notificationType);
			notificationType = NotificationType.None;
			//AudioControl.Instance.ResumeMusic();
		} busy = false;
	}

	public bool GetBusy() {
		return busy;
	}
}

public interface ICollectibleManager {
	void Display(string name, float transitionTime = 0);
}