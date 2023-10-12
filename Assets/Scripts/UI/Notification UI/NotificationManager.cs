using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour {

    [SerializeField] private float scaleConstraint = 20;

    public enum NotificationType {
        None,
        PolaroidClaimed,
        IDCardClaimed,
        SideRoomKeyClaimed,
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
    } 
    private Queue<NotificationCall> notificationQueue;
    private NotificationCall currCall;

    private NotificationObject notificationObject;
    public TextMeshProUGUI NotificationText { get; private set; }
    void Awake() {
        notificationObject = GetComponentInChildren<NotificationObject>(true);
        notificationObject.OnNotificationFinished += DisplayNotification;
        NotificationText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    void DisplayNotification() {
        string message;
        if (currCall.type > 0) {
            NotificationText.gameObject.SetActive(true);
            switch (currCall.type) {
                case NotificationType.PolaroidClaimed:
                    message = "Polaroid and ID Card Claimed";
                    NotificationText.text = message;
                    break;
                case NotificationType.CollectibleRedundant:
                    message = "The Item Is Already Taken";
                    NotificationText.text = message;
                    break;
                case NotificationType.RoomCode:
                    message = "Sector " + currCall.roomCode;
                    NotificationText.text = message;
                    break;
                case NotificationType.SideRoomKeyClaimed:
                    message = "Special Key Claimed";
                    NotificationText.text = message;
                    break;
            } 
            notificationObject.gameObject.SetActive(true);
            notificationObject.Initialize(NotificationText, scaleConstraint);
        }
    }

    public void AddNotification(NotificationType type, string roomCode = null) {
        currCall = new NotificationCall(type, roomCode);
        DisplayNotification();
	}
}
