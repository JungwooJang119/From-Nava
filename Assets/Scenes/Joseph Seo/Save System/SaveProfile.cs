using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveProfile
{
    private string profileName;
    private float playtime;
    private string sector;

    // The following dictionary will cover collectibles (Polaroids, Keys, Lab Reports), enemy status (Dead or Alive), spell unlock progress
    private Dictionary<string, bool> bools = new Dictionary<string, bool>(); 

    // Getters and Setters
    public string GetProfileName() {
        return profileName;
    }

    public Dictionary<string, bool> GetBoolsDictionary() {
        return bools;
    }

    public bool GetBool(string name, bool defaultVal = false) {
        return bools.GetValueOrDefault(name, defaultVal);
    }

    public void SetBool(string name, bool value) {
        bools[name] = value;
    }

    public void Save() {
        SaveSavableData();
    }

    public void SaveSavableData() {
        foreach (ISavable s in GetCachedSavables()) {
            s.Save();
        }
    }

    private List<ISavable> GetCachedSavables() {
        List<ISavable> savables = new List<ISavable>();
        foreach (ISavable s in GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>()) {
            savables.Add(s);
        }
        return savables;
    }

}
