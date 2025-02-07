using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager> {

    private List<Enemy> engagedEnemies;
    private bool combatStance;
    private bool bossEngaged;

    void Awake() {
        engagedEnemies = new List<Enemy>();
        //DontDestroyOnLoad(gameObject);
        InitializeSingleton(gameObject);
    }

    /// <summary>
    /// Register an enemy as "in combat";
    /// </summary>
    /// <param name="enemy"> The enemy registering for combat; </param>
    /// <param name="engaged"> Whether the enemy engaged in combat (true), or disengaged (false); </param>
    public void RegisterEnemy(Enemy enemy, bool engaged) {
        if (engaged && !engagedEnemies.Contains(enemy))  {
            engagedEnemies.Add(enemy);
            if (!bossEngaged && enemy.gameObject.GetComponent<MeleeMiniBossController>() != null) { //Check if boss is entering the radius
                bossEngaged = true;
            } 
        } else {
            engagedEnemies.Remove(enemy); 
            if (bossEngaged && enemy.gameObject.GetComponent<MeleeMiniBossController>() != null) { //Check if boss is leaving the radius
                bossEngaged = false;
            }
        }
        ValidateCombatStance();
    }

    private void ValidateCombatStance() {
        if (combatStance && engagedEnemies.Count == 0) SwitchStance(false);
        else if (!combatStance && engagedEnemies.Count > 0) SwitchStance(true);
    }

    private void SwitchStance(bool embattle) {
        if (AudioControl.Instance == null) return;
        combatStance = embattle;
        TrackMode trackMode = combatStance ? TrackMode.Battle : TrackMode.Exploration;
        if (bossEngaged && combatStance) trackMode = TrackMode.Main;  //Fix this later
        AudioControl.Instance.InterpolateMusicTracks(trackMode);
    }
}
