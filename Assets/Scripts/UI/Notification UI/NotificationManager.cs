using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    public enum NotificationType {
        None,
        PolaroidClaimed,
		CollectibleRedundant,
    }

    private Notification notification;

    // Start is called before the first frame update
    void Start() {
        notification = GetComponentInChildren<Notification>(true);
    }

    public void Notify(NotificationType type) {
		if (!notification.gameObject.activeSelf && type != NotificationType.None) {
            switch (type) {
                case NotificationType.PolaroidClaimed:
                    notification.SetMessage("Polaroid Claimed");
                    notification.SetWidth(2f);
                    break;
                case NotificationType.CollectibleRedundant:
                    notification.SetMessage("The Item Is Already Taken");
					notification.SetWidth(2.75f);
					break;
            } notification.gameObject.SetActive(true);
		}
	}
}
