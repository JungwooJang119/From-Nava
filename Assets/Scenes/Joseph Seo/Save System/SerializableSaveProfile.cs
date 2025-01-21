using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableSaveProfile
{
    public string profileName;

    // Dictionaries of saved data
    public string[] boolKeys;
    public bool[] boolValues;
    public string[] collectibleBoolKeys;
    public bool[] collectibleBoolValues;

    
    // Converts from SaveProfile to SerializableSaveProfile
    public static SerializableSaveProfile FromSaveProfile(SaveProfile saveProfile) {
        if (saveProfile == null) return null;

        SerializableSaveProfile ssp = new SerializableSaveProfile();

        ssp.profileName = saveProfile.GetProfileName();
        ssp.boolKeys = saveProfile.GetBoolsDictionary().Keys.ToArray();
        ssp.boolValues = saveProfile.GetBoolsDictionary().Values.ToArray();
        ssp.collectibleBoolKeys = saveProfile.GetCollectibleBoolsDictionary().Keys.ToArray();
        ssp.collectibleBoolValues = saveProfile.GetCollectibleBoolsDictionary().Values.ToArray();
        
        return ssp;
    }

    // Done when loading in from data
    public SaveProfile ToSaveProfile() {
        SaveProfile sp = new SaveProfile(profileName);
        Dictionary<string, bool> bools = new Dictionary<string, bool>(boolKeys.Length);
        for (int i = 0; i < boolKeys.Length; i++) {
            bools.Add(boolKeys[i], boolValues[i]);
        }
        sp.SetBoolsDictionary(bools);
        Dictionary<string, bool> collectibleBools = new Dictionary<string, bool>(collectibleBoolKeys.Length);
        for (int i = 0; i < collectibleBoolKeys.Length; i++) {
            collectibleBools.Add(collectibleBoolKeys[i], collectibleBoolValues[i]);
        }
        sp.SetCollectibleBoolsDictionary(collectibleBools);
        return sp;
    }
}
