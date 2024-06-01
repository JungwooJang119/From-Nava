#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX) || NAVA_DRM_FREE
#define DISABLESTEAMWORKS
#endif

using System;
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
    private static readonly AppId_t AppID = (AppId_t)2721750;
    private static bool _everInitialized = false;
    
    private bool _initialized = false;
    public static bool Initialized => Instance._initialized;
    
    // Copied from boilerplate. Should help handle SteamAPI warning messages from Steam
    private SteamAPIWarningMessageHook_t _steamAPIWarningMessageHook;
    [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
    protected static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText) {
        Debug.LogWarning(pchDebugText);
    }

    private void Awake() {
        // Singleton behavior as employed by Travis and Chase
        DontDestroyOnLoad(gameObject);
        InitializeSingleton(gameObject);

        if (_everInitialized) {
            // THIS IS ALMOST ALWAYS AN ERROR
            // Don't call Steamworks functions in OnDestroy, always prefer OnDisable if possible
            throw new System.Exception("Tried to Initialize the SteamAPI twice in one session!");
        }
        
        // Boilerplate tests
        if (!Packsize.Test()) {
            Debug.LogError("[Steamworks] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }
        if (!DllCheck.Test()) {
            Debug.LogError("[Steamworks] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }
        
        // Restart app with Steam if not already launched via Steam
        try {
            if (SteamAPI.RestartAppIfNecessary(AppID)) {
                #if UNITY_STANDALONE
                Application.Quit();
                #endif

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                
                return;
            }
        } catch (System.DllNotFoundException e) { // Catch potential issues with DLLs early
            Debug.LogError("[Steamworks] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);
            #if UNITY_STANDALONE
            Application.Quit();
            #endif
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            
            return;
        }
        
        /*
         * Initializes Steamworks. If it fails, the reason can fall into one of these issues:
         * - The Steam client isn't running.
         * - The Steam client couldn't figure out the AppID of the game.
         * 
         * Valve's documentation for this is located here:
         * - https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
         */
        _initialized = SteamAPI.Init();
        if (!_initialized) {
            Debug.LogError("[Steamworks] SteamAPI.Init() failed. Refer to documentation via Valve or commented information above this line.");
            return;
        }

        _everInitialized = true;
    }
    
    // Handles the case where the SteamManager is first loaded and/or Assembly reloads.
    // NEVER DISABLE THE STEAMWORKS MANAGER YOURSELF!
    private void OnEnable() {
        if (Instance == null) {
            InitializeSingleton(gameObject);
        }

        if (!_initialized) {
            return;
        }
        
        // More SteamAPI warning messages boilerplate
        // In order to receive warnings, you MUST launch with "-debug_steamapi" in the launch args
        if (_steamAPIWarningMessageHook == null) {
            _steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            SteamClient.SetWarningMessageHook(_steamAPIWarningMessageHook);
        }
    }
    
    /*
     * According to Steamworks.NET boilerplate:
     * OnApplicationQuit gets called too early to shutdown the SteamAPI
     * Because SteamManager is persistent and never disabled/destroyed, we can shutdown the SteamAPI here
     * Therefore, PLEASE make sure that Steamworks work is not done in any other OnDestroy functions, as execution order
     * is not guaranteed when shutdown occurs.
     * Prefer OnDisable for the aforementioned purpose.
     */
    private void OnDestroy() {
        if (Instance != this) {
            return;
        }

        _instance = null;
        if (!_initialized) {
            return;
        }
        SteamAPI.Shutdown();
    }

    private void Update() {
        if (!_initialized) {
            return;
        }
        
        SteamAPI.RunCallbacks();
    }
    
    #else
    
    public static bool Initialized => false;
    
    #endif // !DISABLESTEAMWORKS
}
