using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveProfile
{
    private string _profileName;
    private float playtime;
    private string _playerLocation;
    private int _playerHealth;
    private int _numberOfPolaroidsCollected;
    private float percentageComplete;
    private int totalCollectibles = 0;
    private int collectedCollectibles = 0;

    // The following dictionary will cover collectibles (Polaroids, Keys, Lab Reports), enemy status (Dead or Alive), spell unlock progress
    private Dictionary<string, int> _enemyHealthDict = new Dictionary<string, int>();
    // The status of if collectibles were collected or not. This includes Polardois, Keys, Lab Reports, and Spells
    private Dictionary<string, bool> _collectibleCollectedDict = new Dictionary<string, bool>();
    // The status of if a collectible was set active or not
    private Dictionary<string, bool> _collectibleActiveDict = new Dictionary<string, bool>();
    // The status of all doors in the scene
    private Dictionary<string, bool> _doorDictionary = new Dictionary<string, bool>();
    // The status of all room controls in the scene
    private Dictionary<string, bool> _roomDictionary = new Dictionary<string, bool>();
    // The status of every cutscene
    private Dictionary<string, bool> _cutsceneDictionary = new Dictionary<string, bool>();

    // This profile should not be called unless something goes horribly wrong.
    public SaveProfile() {
        _profileName = "soreno";
    }

    public SaveProfile(string name) {
        _profileName = name;
    }

    public void Save() {
        Debug.Log("Saving all the data!");
        SaveSavableData();
        Resources.UnloadUnusedAssets();
    }

    public void Load() {
        Debug.Log("Loading all the data!");
        LoadSavableData();
    }

    private List<ISavable> GetCachedSavables() {
        List<ISavable> savables = new List<ISavable>();
        foreach (ISavable s in GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>()) {
            savables.Add(s);
        }
        
        return savables;
    }

    public void SaveSavableData() {
        foreach (ISavable s in GetCachedSavables()) {
            s.Save();
            // Debug.Log("Print Dic")
        }
        // totalCollectibles = 
        // percentageComplete = (float)collectedCollectibles / totalCollectibles;
        // Debug.Log($"Collected: {collectedCollectibles} Total: {totalCollectibles} Percentage Complete: {percentageComplete}");
    }

    private void LoadSavableData() {
        List<ISavable> savables = GetCachedSavables();
        
        foreach (ISavable s in savables) {
            s.Load(this); // Why did they require a SaveProfile like in here?
        }
    }
    
    # region Getters and Setters
    // For Profile Parameters
    public string GetProfileName() {
        return _profileName;
    }

    public void SetName(string name) {
        _profileName = name;
    }

    public int GetPlayerHealth(int defaultVal = 0) {
        return (_playerHealth == 0)? defaultVal : _playerHealth;
    }

    public void SetPlayerHealth(int health) {
        _playerHealth = health;
    }

    public string GetPlayerLocation(string defaultVal = "B2Center") {
        return (_playerLocation != null)? _playerLocation : defaultVal;
    }

    public void SetPlayerLocation(string location) {
        _playerLocation = location;
    }
    public float GetPlaytime() {
        return playtime;
    }

    public void SetPlaytime(float input) {
        playtime = input;
    }

    public int GetNumberOfPolaroids() {
        return _numberOfPolaroidsCollected;
    }

    public void SetNumberOfPolaroids(int defaultVal = 0) {
        _numberOfPolaroidsCollected = defaultVal;
    }

    public float GetPercentage() {
        return percentageComplete;
    }

    public void SetPercentage(float percent) {
        percentageComplete = percent;
    }

    public void IncrementTotalNumber(int n = 0) {
        totalCollectibles += n;
    }

    public void IncrementCollectedNumber(int n = 0) {
        collectedCollectibles += n;
    }

    // For Enemy Health
    public Dictionary<string, int> GetEnemyHealthDictionary() {
        return _enemyHealthDict;
    }

    public void SetEnemyHealthDictionary(Dictionary<string, int> intDict) {
        _enemyHealthDict = intDict;
    }

    public int GetEnemyHealth(string name, int defaultVal = 0) {
        return _enemyHealthDict.GetValueOrDefault(name, defaultVal);
    }

    public void SetEnemyHealth(string name, int value) {
        _enemyHealthDict[name] = value;
    }

    // For Collectibles, if they were collected
    public Dictionary<string, bool> GetCollectibleCollectedDictionary() {
        return _collectibleCollectedDict;
    }

    public void SetCollectibleCollectedDictionary(Dictionary<string, bool> boolsDict) {
        _collectibleCollectedDict = boolsDict;
    }

    public bool GetCollectibleCollected(string name, bool defaultVal = false) {
        return _collectibleCollectedDict.GetValueOrDefault(name, defaultVal);
    }

    public void SetCollectibleCollected(string name, bool value) {
        _collectibleCollectedDict[name] = value;
    }

    // For Collectibles, if they were set active. Necessary for chests and spells, for if they were set active but not collected
    public Dictionary<string, bool> GetCollectibleActiveDictionary() {
        return _collectibleActiveDict;
    }

    public void SetCollectibleActiveDictionary(Dictionary<string, bool> boolsDict) {
        _collectibleActiveDict = boolsDict;
    }

    public bool GetCollectibleActive(string name, bool defaultVal = false) {
        return _collectibleActiveDict.GetValueOrDefault(name, defaultVal);
    }

    public void SetCollectibleActive(string name, bool value) {
        _collectibleActiveDict[name] = value;
    }

    // For Doors, and their active status.
    public Dictionary<string, bool> GetDoorDictionary() {
        return _doorDictionary;
    }
    
    public void SetDoorDictionary(Dictionary<string, bool> boolsDict) {
        _doorDictionary = boolsDict;
    }

    public bool GetDoor(string name, bool defaultVal = false) {
        return _doorDictionary.GetValueOrDefault(name, defaultVal);
    }

    public void SetDoor(string name, bool value) {
        _doorDictionary[name] = value;
    }

    // For Rooms, and their completition status
    public Dictionary<string, bool> GetRoomControlDictionary() {
        return _roomDictionary;
    }
    
    public void SetRoomControlDictionary(Dictionary<string, bool> boolsDict) {
        _roomDictionary = boolsDict;
    }

    public bool GetRoomControl(string name, bool defaultVal = false) {
        return _roomDictionary.GetValueOrDefault(name, defaultVal);
    }

    public void SetRoomControl(string name, bool value) {
        _roomDictionary[name] = value;
    }

    // For Cutscenes, and their completition status
    public Dictionary<string, bool> GetCutsceneDictionary() {
        return _cutsceneDictionary;
    }
    
    public void SetCutsceneDictionary(Dictionary<string, bool> boolsDict) {
        _cutsceneDictionary = boolsDict;
    }

    public bool GetCutscene(string name, bool defaultVal = false) {
        return _cutsceneDictionary.GetValueOrDefault(name, defaultVal);
    }

    public void SetCutscene(string name, bool value) {
        _cutsceneDictionary[name] = value;
    }

    # endregion
}
