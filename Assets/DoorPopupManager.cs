using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPopupManager : ItemManager<DoorData> {

    // Attached Report Manager in Editor to use to display just text vs text and images.
    [SerializeField] private ReportManager reportManager;

    // Override the ItemManager version of Display;
    // If there is an image, use the ItemManager version
    // Else use the attached ReportManager to display it via just text.
    public override void Display(DoorData itemData, float transitionTime = 0) {
        if (itemData.sprite != null) {
            base.Display(itemData, transitionTime);
        } else {
            reportManager.DisplayString(itemData.text);
        }
    }

    // Overrides the AddItem function in CollectibleManager so that DoorData is not added, thereby enabling the player to reread doors
    protected override bool AddItem(DoorData item) {
        return true;
    }

}
