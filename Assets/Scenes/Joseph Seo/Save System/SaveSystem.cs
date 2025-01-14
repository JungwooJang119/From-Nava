using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    // References
    private static SaveProfile[] saveProfiles = new SaveProfile[3];
    private static SaveProfile current;
    private static int currentIndex = -1;

    public static SaveProfile Current {
        get {
            if (current == null) {
                current = new SaveProfile();
                currentIndex = -1;
            }
            return current;
        }
        private set {
            current = value; // (Joseph 1 / 14 / 25) What is value? I have no clue
        }
    }

    public static SaveProfile GetProfile(int index) {
        return saveProfiles[index];
    }

    public static void SaveGame(string reason = "") {
        Current.Save();
    }

    // public static void LoadSaveProfile(int index) {
    //     SaveSystem.SetCurrentProfile(index);

    // }

    
}
