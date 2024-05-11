using System.Linq;
using UnityEngine;
using TMPro;

public enum NotificationType { None = 0, CollectibleRedundant, RoomCode,
                               PolaroidClaimed, ReportClaimed, SideRoomKeyClaimed }

public class NotificationManager : MonoBehaviour {

    [SerializeField] private float scaleConstraint = 20;

    private class NotificationCall {
        public NotificationType type;
        public string roomCode = null;
        public NotificationCall(NotificationType type, string roomCode) {
            this.type = type;
            this.roomCode = roomCode;
        }
    }
    private NotificationType delayedType = NotificationType.None;
    private NotificationCall currCall;

    private NotificationObject notificationObject;
    public TextMeshProUGUI NotificationText { get; private set; }

    void Awake() {
        notificationObject = GetComponentInChildren<NotificationObject>(true);
        notificationObject.OnNotificationFinished += DisplayNotification;
        NotificationText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void Init(CollectibleController controller) {
        controller.OnClaimResult += Controller_OnClaimResult;
        controller.OnCallsEnd += Controller_OnCallsEnd;
    }

    private void Controller_OnClaimResult(ItemCall call) {
        if (call.output != null && call.output.Count > 0) {
            System.Type itemType = call.output.ToArray()[0].GetType();
            delayedType = itemType == typeof(PolaroidData) ? NotificationType.PolaroidClaimed
                        : itemType == typeof(ReportData) ? NotificationType.ReportClaimed
                        : itemType == typeof(SideRoomKeyData) ? NotificationType.SideRoomKeyClaimed
                                                              : delayedType;
        } else if (delayedType == 0) delayedType = NotificationType.CollectibleRedundant;
    }

    private void Controller_OnCallsEnd() {
        AddNotification(delayedType);
        delayedType = NotificationType.None;
    }

    private void DisplayNotification() {
        string message;
        if (currCall.type > 0) {
            NotificationText.gameObject.SetActive(true);
            switch (currCall.type) {
                case NotificationType.PolaroidClaimed:
                    message = "Polaroid and ID Card Claimed";
                    NotificationText.text = message;
                    break;
                case NotificationType.ReportClaimed:
                    message = "Lab Report Claimed";
                    NotificationText.text = message;
                    break;
                case NotificationType.SideRoomKeyClaimed:
                    message = "Special Key Claimed";
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
            } notificationObject.gameObject.SetActive(true);
            notificationObject.Initialize(NotificationText, scaleConstraint);
        }
    }

    public void AddNotification(NotificationType type, string roomCode = null) {
        currCall = new NotificationCall(type, roomCode);
        DisplayNotification();
	}
}
