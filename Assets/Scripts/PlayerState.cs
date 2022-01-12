using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected PlayerState[] incompatable;
    protected PlayerState[] states;
    
    protected virtual void Awake() {
        states = GetComponents<PlayerState>();
    }

    public virtual void Disable() {
        enabled = false;
    }

    public virtual void Enable()
    {
        foreach (var state in states)
            state.enabled = true;
        foreach(var state in incompatable) {
            state.Disable();
        }
    }
}
