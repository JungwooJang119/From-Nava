using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableSaveProfile
{
    public string profileName;

    // Dictionaries of saved data
    public string[] _enemyHealthKey;
    public int[] _enemyHealthValue;
    public string[] _collectedKey;
    public bool[] _collectedValue;
    public string[] _activeKey;
    public bool[] _activeValue;
    public string[] _doorKey;
    public bool[] _doorValue;
    public string[] _roomKey;
    public bool[] _roomValue;

    // Converts from SaveProfile to SerializableSaveProfile
    public static SerializableSaveProfile FromSaveProfile(SaveProfile saveProfile) {
        if (saveProfile == null) return null;

        SerializableSaveProfile ssp = new SerializableSaveProfile();

        ssp.profileName = saveProfile.GetProfileName();

        ssp._enemyHealthKey = saveProfile.GetEnemyHealthDictionary().Keys.ToArray();
        ssp._enemyHealthValue = saveProfile.GetEnemyHealthDictionary().Values.ToArray();
        ssp._collectedKey = saveProfile.GetCollectibleCollectedDictionary().Keys.ToArray();
        ssp._collectedValue = saveProfile.GetCollectibleCollectedDictionary().Values.ToArray();
        ssp._activeKey = saveProfile.GetCollectibleActiveDictionary().Keys.ToArray();
        ssp._activeValue = saveProfile.GetCollectibleActiveDictionary().Values.ToArray();
        ssp._doorKey = saveProfile.GetDoorDictionary().Keys.ToArray();
        ssp._doorValue = saveProfile.GetDoorDictionary().Values.ToArray();
        ssp._roomKey = saveProfile.GetRoomControlDictionary().Keys.ToArray();
        ssp._roomValue = saveProfile.GetRoomControlDictionary().Values.ToArray();

        return ssp;
    }

    // Done when loading in from data
    public SaveProfile ToSaveProfile() {
        SaveProfile sp = new SaveProfile(profileName);

        Dictionary<string, int> enemyHealth = new Dictionary<string, int>(_enemyHealthKey.Length);
        for (int i = 0; i < _enemyHealthKey.Length; i++) {
            enemyHealth.Add(_enemyHealthKey[i], _enemyHealthValue[i]);
        }
        sp.SetEnemyHealthDictionary(enemyHealth);

        Dictionary<string, bool> collected = new Dictionary<string, bool>(_collectedKey.Length);
        for (int i = 0; i < _collectedKey.Length; i++) {
            collected.Add(_collectedKey[i], _collectedValue[i]);
        }
        sp.SetCollectibleCollectedDictionary(collected);

        Dictionary<string, bool> active = new Dictionary<string, bool>(_activeKey.Length);
        for (int i = 0; i < _activeKey.Length; i++) {
            active.Add(_activeKey[i], _activeValue[i]);
        }
        sp.SetCollectibleActiveDictionary(active);

        Dictionary<string, bool> door = new Dictionary<string, bool>(_doorKey.Length);
        for (int i = 0; i < _doorKey.Length; i++) {
            door.Add(_doorKey[i], _doorValue[i]);
        }
        sp.SetDoorDictionary(door);

        Dictionary<string, bool> room = new Dictionary<string, bool>(_roomKey.Length);
        for (int i = 0; i < _roomKey.Length; i++) {
            room.Add(_roomKey[i], _roomValue[i]);
        }
        sp.SetRoomControlDictionary(room);

        return sp;
    }
}
