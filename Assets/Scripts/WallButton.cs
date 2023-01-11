using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// button that uses ontriggerenter instead of on collision enter
/// </summary>
public class WallButton : Button
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Projectile"))
        {
            if (currentlyWhite != amWhite)
            {
                audioSource.Play();
                OnPush(amWhite);

            }
        }
    }
    protected override void Activate() { return; }
    protected override void Deactivate() { return; }
}
