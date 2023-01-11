using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// used to solve the problem of which movement abiliites a player can use at any given time
/// different movement abilities are represented as "states" and stored in a compatability matrix
/// when a certain state is enabled, so are all of the other player states that are compatable
/// </summary>
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
