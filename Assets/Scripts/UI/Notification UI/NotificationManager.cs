using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour {

    [SerializeField] private float scaleConstraint = 20;

    public enum NotificationType {
        None,
        PolaroidClaimed,
		CollectibleRedundant,
        RoomCode
    }

    private class NotificationCall {
        public NotificationType type;
        public string roomCode = null;
        public NotificationCall(NotificationType type, string roomCode) {
            this.type = type;
            this.roomCode = roomCode;
        }
    } private Queue<NotificationCall> notificationQueue;

    private NotificationObject notificationObject;
    public TextMeshProUGUI NotificationText { get; private set; }
    void Awake() {
        notificationObject = GetComponentInChildren<NotificationObject>(true);
        notificationObject.OnNotificationFinished += DisplayNotification;
        NotificationText = GetComponentInChildren<TextMeshProUGUI>(true);
        notificationQueue = new Queue<NotificationCall>();
    }

    void DisplayNotification() {
        if (notificationQueue.Count > 0) {
            NotificationCall nextCall = notificationQueue.Dequeue();
            string message;
            if (nextCall.type > 0) {
                NotificationText.gameObject.SetActive(true);
                switch (nextCall.type) {
                    case NotificationType.PolaroidClaimed:
                        message = "Polaroid Claimed";
                        NotificationText.text = message;
                        break;
                    case NotificationType.CollectibleRedundant:
                        message = "The Item Is Already Taken";
                        NotificationText.text = message;
                        break;
                    case NotificationType.RoomCode:
                        message = "Sector " + nextCall.roomCode;
                        NotificationText.text = message;
                        break;
                } notificationObject.gameObject.SetActive(true);
                notificationObject.Initialize(NotificationText, scaleConstraint);
            }
        }
    }

    public void AddNotification(NotificationType type, string roomCode = null) {
        notificationQueue.Enqueue(new NotificationCall(type, roomCode));
        if (!notificationObject.gameObject.activeSelf) DisplayNotification();
	}
}
