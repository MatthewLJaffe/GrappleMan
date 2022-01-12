using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCollider : MonoBehaviour
{
    private void Awake()
    {
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.size = _spriteRenderer.size;
    }
}
