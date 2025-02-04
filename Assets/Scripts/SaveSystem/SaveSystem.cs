using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem
{
    // References
    private static SaveProfile[] saveProfiles = new SaveProfile[3];
    private static SaveProfile current;
    private static int currentIndex = 1;
    private static SerializablePlayerSettings playerSettings;

    // Remove this later if we do this as JSON
    public static DeserializationTypeRemapBinder AssemblyRemapBinder
    {
        get
        {
            if (_assemblyRemapBinder == null)
            {
                _assemblyRemapBinder = new DeserializationTypeRemapBinder();
                _assemblyRemapBinder.AddAssemblyMapping("Assembly-CSharp", "NavaScripts"); // Check what this does fml
            }

            return _assemblyRemapBinder;
        }
    }
    private static DeserializationTypeRemapBinder _assemblyRemapBinder;

    // I think this converts everything seriazable to regular profiles and sets it for when saveSystem is first initialized
    public SaveSystem() {
        // Debug.Log("Constructor Called");
        SetProfile(0, GetSerializableSaveProfile(0)?.ToSaveProfile());
        SetProfile(1, GetSerializableSaveProfile(1)?.ToSaveProfile());
        SetProfile(2, GetSerializableSaveProfile(2)?.ToSaveProfile());
        playerSettings = LoadSerializablePlayerSettings();
    }

    public static SerializablePlayerSettings LoadSerializablePlayerSettings() {
        Debug.Log("Save System Loading in SerializablePlayerSettings");
        string path = Application.persistentDataPath + "/navaSettings.lab";
        if (File.Exists(path)) {
            Debug.Log("Path does exist");
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = AssemblyRemapBinder;
            
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SerializablePlayerSettings settings = formatter.Deserialize(stream) as SerializablePlayerSettings;
            stream.Close();

            return settings;
        } else {
            // TODO: Create a new SerializablePlayerSettings based on the default Audio Settings
            Debug.Log("Creating a new SerializablePlayerSettings");
            return new SerializablePlayerSettings();
        }
    }

    public static void SaveSerializablePlayerSettings() {
        Debug.Log("Save System called SaveSerializablePlayerSettings");
        string path = Application.persistentDataPath + "/navaSettings.lab";
        BinaryFormatter formatter = new();
        formatter.Binder = AssemblyRemapBinder;

        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, playerSettings);

        stream.Close();
    }

    public static SerializablePlayerSettings GetSettings() {
        return playerSettings;
    }

    public static void DeleteSaveProfile(int index) {
        // Debug.Log($"Deleting Save Profile{index}");
        string path = GetFilePath(index);
        if (File.Exists(path)) {
            File.Delete(path);
            SetProfile(index, null);
            // Debug.Log("Deleted Sucessfully!");
        } else {
            // Debug.Log("Couldn't delete: Path does not exist!");
        }
        

    }

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

    public static void LoadSaveProfile(int index) {
        SaveSystem.SetCurrentProfile(index);
    }

    public static void SetCurrentProfile(int index) {
        currentIndex = index;
        Current = GetProfile(index);
    }

    private static SerializableSaveProfile LoadFromFile(string path) {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = AssemblyRemapBinder;
            
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SerializableSaveProfile profile = formatter.Deserialize(stream) as SerializableSaveProfile;
            stream.Close();

            return profile;
        } else {
            return null;
        }
    }

    public static SerializableSaveProfile GetSerializableSaveProfile(int index) {
        string path = GetFilePath(index);
        return LoadFromFile(path);
    }



    public static void SaveGame() {
        // Slider has a "reason" if conditional 
        Current.Save();
        SetProfile(currentIndex, Current);
        SerializableSaveProfile profile = SerializableSaveProfile.FromSaveProfile(Current);
        string path = GetFilePath(currentIndex);
        SaveToFile(profile, path);
        SaveSerializablePlayerSettings();
    }

    // Actually loading the Save File
    private static void SaveToFile(SerializableSaveProfile profile, string path) {
        // I want to try to switch this save system to JSON
        BinaryFormatter formatter = new();
        formatter.Binder = AssemblyRemapBinder;

        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, profile);

        stream.Close();
    }

    // GetFilePath
    public static string GetFilePath(int index) {
        // I'm not sure how this fucking works tbh imma try not
        return Application.persistentDataPath + string.Format("/nava{0}.lab", index);
    }



    #region Getters / Setters
    public static SaveProfile GetProfile(int index) {
        return saveProfiles[index];
    }

    public static void SetProfile(int index, SaveProfile profile) {
        saveProfiles[index] = profile;
    }

    // public static void LoadSaveProfile(int index) {
    //     SaveSystem.SetCurrentProfile(index);

    // }

    #endregion 
}
