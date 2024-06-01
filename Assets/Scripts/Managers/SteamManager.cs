#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX) || NAVA_DRM_FREE
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
using System.Collections;
#endif

/// <summary>
/// Manager singleton invoking Steamworks via Steamworks.NET v20.2.0
/// </summary>

[DisallowMultipleComponent]
public class SteamManager : Singleton<SteamManager>
{
    #if !DISABLESTEAMWORKS
    void Awake() {
        DontDestroyOnLoad(gameObject);
        InitializeSingleton(gameObject);
        if (!Packsize.Test()) {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }
        if (!DllCheck.Test()) {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }

        try {
            if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid)) {
                #if UNITY_STANDALONE
                Application.Quit();
                #endif

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                
                return;
            }
        } catch (System.DllNotFoundException e) {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);
            #if UNITY_STANDALONE
            Application.Quit();
            #endif
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            
            return;
        }
        
        
    }
    #endif
}
