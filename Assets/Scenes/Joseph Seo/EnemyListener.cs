using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListener : Listener
{
    [SerializeField] List<Enemy> enemies;

    void Awake() {
        foreach (Enemy enemy in enemies) enemy.OnDeath += OnStatusChange;
    }

    // Whenever an enemy dies, check every enemy to see if there are any still alive. If not, invoke the event and set this component to true.
    protected override void OnStatusChange() {
        foreach (Enemy enemy in enemies) {
            if (enemy.CheckIsAlive()) {
                return;
            }
        }
        status = true;
        OnListen?.Invoke();
    }
}