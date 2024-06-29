using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Listener : MonoBehaviour
{
    // Listeners both serve as tools to check if the puzzle has been completed and as bits of puzzle logic themselves.
    // Enemy, Firewood, and Collectible Listener serve primarily as the former
    // PressurePlate and Fan Listener serve as the latter: both can be used as permanent solutions by inserting them into a Room Control script
    // or as temporary bits of puzzle to toggle doors on and off.
    public System.Action OnListen;                  // Event sent by all listeners when the puzzle status has changed (usually for the better)
    protected bool status;                          // Status of the listener. True means solved
    public bool CheckStatus() {
        return status;
    }
    protected abstract void OnStatusChange();       // Abstract function meant to force the usage of OnListen. Not strictly necessary, and can be removed.
}
