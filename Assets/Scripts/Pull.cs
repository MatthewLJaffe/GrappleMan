using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// dash movement player does when grappling to pull knob
/// </summary>
public class Pull : PlayerState
{
    [SerializeField] private float pullSpeed;
    private Rigidbody2D rb;
    private Vector2 pullPoint = NullVector.empty;
    private Vector2 direction = NullVector.empty;
    private float distanceThreshold = .25f;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        PullKnob.OnPull += StartPull;
    }
    private void OnDestroy()
    {
        PullKnob.OnPull -= StartPull;
    }

    public void StartPull(Vector2 point)
    {
        if (point != NullVector.empty)
        {
            Enable();
            rb.gravityScale = 0;
            pullPoint = point;
            direction = (point - (Vector2)transform.position).normalized;
            rb.velocity = direction * pullSpeed;
            StartCoroutine(WaitforDash(Vector2.Distance(point, transform.position) / pullSpeed));
        }
    }

    private void FixedUpdate()
    {
        if (direction != NullVector.empty && Vector2.Distance(pullPoint, transform.position) < distanceThreshold)
        {
            rb.velocity = Vector2.zero;
            PullKnob.OnPull(NullVector.empty);
            Dash.OnDash(direction);
            direction = NullVector.empty;
        }
    }

    private IEnumerator WaitforDash(float time)
    {
        yield return new WaitForSeconds(time);
        if (direction != NullVector.empty)
        {
            rb.velocity = Vector2.zero;
            PullKnob.OnPull(NullVector.empty);
            Dash.OnDash(direction);
            direction = NullVector.empty;
        }
    }
}
