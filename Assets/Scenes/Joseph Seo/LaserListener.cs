using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserListener : Listener
{
    [SerializeField] private LaserTerminal terminal;

    void Awake() {
        terminal.OnFinish += OnStatusChange;
    }
    protected override void OnStatusChange() {
        status = true;
        OnListen?.Invoke();
    }
}
