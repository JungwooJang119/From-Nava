using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDisablerControl : Singleton<RoomDisablerControl>
{
    private PlayerController pc;
    private GameObject player;

    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] rooms;

    void Awake() {
		DontDestroyOnLoad(gameObject);
		InitializeSingleton(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
        string roomCode = "";
        for (int i = 0; i < spawnPoints.Length; i++) {
            if (spawnPoints[i].name.Substring(0, 2) != pc.spawn.name.Substring(0,2)) {
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
                          .AddNotification(NotificationManager.NotificationType.RoomCode, roomCode);
                        break;
                    }
                }
            }
        } 
    }
}
