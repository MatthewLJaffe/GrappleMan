using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// player state for when player has grappled to launching circle
/// </summary>
public class Dash : PlayerState
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float duration;
    private LateralMove _lateralMove;
    private Vector2 direction = NullVector.empty;
    private Rigidbody2D rb;
    private bool dashing = false;
    private float time = 0;
    public static Action<Vector2> OnDash;

    protected override void Awake() {
        _lateralMove = GetComponent<LateralMove>();
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        OnDash += StartDash;
    }

    private void OnDestroy() {
        OnDash -= StartDash;
    }

    private void StartDash(Vector2 dir)
    {
        if(dir != NullVector.empty)
        {
            Enable();
            direction = dir;
            dashing = true;
        }
    }

    //change speed in dash based on animation curve
    private void FixedUpdate()
    {
        if (dashing)
        {
            if (time < duration)
            {
                rb.velocity = direction * curve.Evaluate(time / duration) * maxSpeed;
                time += Time.fixedDeltaTime;
            }
            else
            {
                time = 0;
                dashing = false;
                direction = NullVector.empty;
                rb.gravityScale = 1;
                OnDash(direction);
                _lateralMove.Enable();
            }
        }
    }
}
