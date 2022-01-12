using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivate : MonoBehaviour
{
    public static Action OnSwitch = delegate { };
    private float disableTime = .1f;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnSwitch();
            StartCoroutine(DisableSwitch());
        }
    }

    private IEnumerator DisableSwitch()
    {
        yield return new WaitForSeconds(disableTime);
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
    }
}
