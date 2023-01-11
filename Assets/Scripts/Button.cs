using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// base class for black / white buttons player can interact with
/// </summary>
public class Button : SwitchOnOff
{
    public static Action<bool> OnPush = delegate { };
    private Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected AudioSource audioSource;

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent) {
            _animator = transform.parent.GetComponent<Animator>();
            _animator.SetBool("StartUp", currentlyWhite != amWhite);
        }
        OnPush += SwitchOn;
    }

    //collision event for pressing button
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.CompareTag("Player") && collision.collider.GetType() == typeof(CircleCollider2D)) 
            || collision.gameObject.CompareTag("Projectile")) {
            if (currentlyWhite != amWhite)
            {
                audioSource.Play();
                OnPush(amWhite);
            }
        }
    }

    protected override void SwitchOn(bool isWhite)
    {
        currentlyWhite = isWhite;
        base.SwitchOn(isWhite);
    }

    protected override void Activate() {
        _animator.SetTrigger("Down");
    }

    protected override void Deactivate() {
        _animator.SetTrigger("Up");
    }

}
