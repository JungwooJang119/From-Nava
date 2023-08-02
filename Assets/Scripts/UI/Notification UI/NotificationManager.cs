using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour {

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
    public static TextMeshProUGUI NotificationText { get; private set; }
    public static string Message { get; private set; }

    void Awake() {
        notificationObject = GetComponentInChildren<NotificationObject>(true);
        NotificationText = GetComponentInChildren<TextMeshProUGUI>(true);
        notificationQueue = new Queue<NotificationCall>();
    }

    void Update() {
        if (notificationQueue.Count > 0) {
            if (!notificationObject.gameObject.activeSelf) {
                NotificationCall nextCall = notificationQueue.Dequeue();
                string message;
                if (nextCall.type > 0) {
                    NotificationText.gameObject.SetActive(true);
                    switch (nextCall.type) {
                        case NotificationType.PolaroidClaimed:
                            message = "Polaroid Claimed";
                            Message = message;
                            NotificationText.text = message;
                            break;
                        case NotificationType.CollectibleRedundant:
                            message = "The Item Is Already Taken";
                            Message = message;
                            NotificationText.text = message;
                            break;
                        case NotificationType.RoomCode:
                            message = "Sector " + nextCall.roomCode;
                            Message = message;
                            NotificationText.text = message;
                            break;
                    } notificationObject.gameObject.SetActive(true);
                }
            }
        }
    }

    public void AddNotification(NotificationType type, string roomCode = null) {
        notificationQueue.Enqueue(new NotificationCall(type, roomCode));
	}
}
