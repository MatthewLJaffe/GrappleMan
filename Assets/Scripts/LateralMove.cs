using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// grounded lateral movement
/// </summary>
public class LateralMove : PlayerState
{
    private Rigidbody2D rb;
    private Animator _animator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedFrames;
    [SerializeField] private float slowFrames;
    [SerializeField] private float airSlowFrames;

    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Start() {
        Enable();
    }

    private void FixedUpdate()
    {
        float slowFactor;
        if (Jump.grounded)
        {
            slowFactor = slowFrames;
            if (Mathf.Abs(rb.velocity.x) > moveSpeed)
                rb.velocity = new Vector2(GetDirection() * moveSpeed, rb.velocity.y);
        }
        else
            slowFactor = airSlowFrames;
        if (Input.GetKey(KeyCode.D))
        {
            if(rb.velocity.x < moveSpeed) 
            {
                _animator.SetBool("xMove", true);
                rb.AddForce(new Vector2(moveSpeed / speedFrames, 0), ForceMode2D.Impulse);
                if (rb.velocity.x > moveSpeed)
                    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if(rb.velocity.x > -moveSpeed)
            {
                _animator.SetBool("xMove", true);
                rb.AddForce(new Vector2(-moveSpeed / speedFrames, 0), ForceMode2D.Impulse);
                if (rb.velocity.x < -moveSpeed)
                    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }   
        }
        else if (rb.velocity.x != 0)
        {
            if (Mathf.Abs(rb.velocity.x) < moveSpeed / slowFactor) {
                _animator.SetBool("xMove", false);
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }
            else
                rb.AddForce(new Vector2(moveSpeed / slowFactor * -GetDirection(), 0), ForceMode2D.Impulse);          
        }
        else
            _animator.SetBool("xMove", false);

    }

    private float GetDirection() {
        return rb.velocity.x / Mathf.Abs(rb.velocity.x);
    }
}
