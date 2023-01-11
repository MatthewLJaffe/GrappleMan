using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// base class for response to on / off states of buttons
/// </summary>
public abstract class SwitchOnOff : MonoBehaviour
{
    [SerializeField] protected bool amWhite;
    protected bool currentlyWhite = false;

    protected virtual void Awake() {
        Button.OnPush += SwitchOn;
        if (currentlyWhite == amWhite)
            Activate();
        else
            Deactivate();
    }

    protected virtual void OnDestroy() {
        Button.OnPush -= SwitchOn;
    }
    protected virtual void SwitchOn(bool isWhite) {
        if (isWhite == amWhite)
            Activate();
        else
            Deactivate();
    }
    protected virtual void Activate() { throw new NotImplementedException(); }
    protected virtual void Deactivate() { throw new NotImplementedException(); }
}
