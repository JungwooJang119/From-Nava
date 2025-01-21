using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveProfile
{
    private string profileName;
    private float playtime;
    private string sector;
    private int playerHealth;

    // The following dictionary will cover collectibles (Polaroids, Keys, Lab Reports), enemy status (Dead or Alive), spell unlock progress
    private Dictionary<string, bool> bools = new Dictionary<string, bool>();
    private Dictionary<string, bool> enemyBools = new Dictionary<string, bool>();
    private Dictionary<string, bool> collectibleBools = new Dictionary<string, bool>();

    public SaveProfile(string name) {
        profileName = name;
    }
    
    public SaveProfile() {
        profileName = "soreno";
    }

    // Getters and Setters
    public string GetProfileName() {
        return profileName;
    }

    public Dictionary<string, bool> GetBoolsDictionary() {
        return bools;
    }

    public Dictionary<string, bool> GetEnemyBoolsDictionary() {
        return enemyBools;
    }

    public void SetEnemyBoolsDictionary(Dictionary<string, bool> boolsDict) {
        enemyBools = boolsDict;
    }

    public Dictionary<string, bool> GetCollectibleBoolsDictionary() {
        return collectibleBools;
    }

    public void SetCollectibleBoolsDictionary(Dictionary<string, bool> boolsDict) {
        collectibleBools = boolsDict;
    }

    public bool GetEnemyBool(string name, bool defaultVal = false) {
        return bools.GetValueOrDefault(name, defaultVal);
    }

    public bool GetCollectibleBool(string name, bool defaultVal = false) {
        return collectibleBools.GetValueOrDefault(name, defaultVal);
    }

    public void SetEnemyBool(string name, bool value) {
        enemyBools[name] = value;
    }
    public void SetCollectibleBool(string name, bool value) {
        collectibleBools[name] = value;
    }

    public void SetBoolsDictionary(Dictionary<string, bool> boolsDict) {
        bools = boolsDict;
    }

    public bool GetBool(string name, bool defaultVal = false) {
        return bools.GetValueOrDefault(name, defaultVal);
    }

    public void SetBool(string name, bool value) {
        bools[name] = value;
    }

    public void SetName(string name) {
        profileName = name;
    }

    public void Save() {
        SaveSavableData();
    }

    public void SaveSavableData() {
        foreach (ISavable s in GetCachedSavables()) {
            s.Save();
            // Debug.Log("Print Dic")
        }
        Debug.Log("Final saved dictionary ends up being this:");
        PrintDictionary();
    }

    private List<ISavable> GetCachedSavables() {
        List<ISavable> savables = new List<ISavable>();
        foreach (ISavable s in GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>()) {
            savables.Add(s);
        }
        
        return savables;
    }

    public void Load() {
        LoadSavableData();
    }

    private void LoadSavableData() {
        List<ISavable> savables = GetCachedSavables();
        
        foreach (ISavable s in savables) {
            s.Load(this); // Why did they require a SaveProfile like in here?
        }
        Debug.Log("Final loaded dictionary looks like this:");
        PrintDictionary();
    }

    public void PrintDictionary() {
        foreach(string s in bools.Keys) {
            Debug.Log(s + " has value " + bools[s]);
        }
        foreach (string s in collectibleBools.Keys) {
            Debug.Log(s + " has value " + collectibleBools[s]);
        }
    }
}
