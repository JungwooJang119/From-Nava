using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDisablerControl : Singleton<RoomDisablerControl> {

    [SerializeField] private PlayerController pc;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] rooms;

    void Awake() {
		InitializeSingleton(gameObject);
        string roomCode;
        for (int i = 0; i < spawnPoints.Length; i++) {
            // (Joseph 1 / 22 / 25) Modifying this next section in order to use the Profile data and not load into a disabled room
            if (spawnPoints[i].name.Substring(0, 2) != SaveSystem.Current.GetPlayerLocation("B2Center").Substring(0, 2)) {
                roomCode = spawnPoints[i].name.Substring(0, 2);
                for (int j = 0; j < rooms.Length; j++) {
                    if (rooms[j].name == roomCode) {
                        rooms[j].SetActive(false);
                    }
                }
            }
        }
    }

    public void ChangeRoomsState(Transform newSpawnPoint) {
        string roomCode;
        for (int i = 0; i < spawnPoints.Length; i++) {
            if (spawnPoints[i].name.Substring(0, 2) != newSpawnPoint.name.Substring(0,2)) {
                roomCode = spawnPoints[i].name.Substring(0, 2);
                for (int j = 0; j < rooms.Length; j++) {
                    if (rooms[j].name.Substring(0, 2) == roomCode) {
                        rooms[j].SetActive(false);
                    }
                }
            } else {
                for (int j = 0; j < rooms.Length; j++) {
                    roomCode = spawnPoints[i].name.Substring(0, 2);
                    if (rooms[j].name.Substring(0, 2) == roomCode && !rooms[j].activeSelf) {
                        rooms[j].SetActive(true);
                        ReferenceSingleton.Instance.collectibleController
                          .GetComponentInChildren<NotificationManager>(true)?
                          .AddNotification(NotificationType.RoomCode, roomCode);
                        break;
                    }
                }
            }
        } 
    }
}
