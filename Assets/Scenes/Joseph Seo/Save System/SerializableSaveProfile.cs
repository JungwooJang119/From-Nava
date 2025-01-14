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
    
    // Converts from SaveProfile to SerializableSaveProfile
    public static SerializableSaveProfile FromSaveProfile(SaveProfile saveProfile) {
        if (saveProfile == null) return null;

        SerializableSaveProfile ssp = new SerializableSaveProfile();

        ssp.profileName = saveProfile.GetProfileName();
        ssp.boolKeys = saveProfile.GetBoolsDictionary().Keys.ToArray();
        ssp.boolValues = saveProfile.GetBoolsDictionary().Values.ToArray();
        
        return ssp;
    }

    // Done when loading in from data
    public SaveProfile ToSaveProfile() {
        SaveProfile sp = new SaveProfile();
        
        return sp;
    }
}
