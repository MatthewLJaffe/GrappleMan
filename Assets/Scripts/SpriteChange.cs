using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : SwitchOnOff
{
    [SerializeField] private Sprite onWhiteSprite;
    [SerializeField] private Sprite offWhiteSprite;
    [SerializeField] private Sprite onBlackSprite;
    [SerializeField] private Sprite offBlackSprite;
    [SerializeField] private float offAlpha;
    [SerializeField] private float onAlpha;

    private Color color;
    private SpriteRenderer _spriteRenderer;

    protected override void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        color = Color.white;
        base.Awake();
    }

    protected override void Activate() {
        if (amWhite)
            _spriteRenderer.sprite = onWhiteSprite;
        else
            _spriteRenderer.sprite = onBlackSprite;
        color.a = onAlpha;
        _spriteRenderer.color = color;
    }

    protected override void Deactivate() {
        if (amWhite)
            _spriteRenderer.sprite = offWhiteSprite;
        else
            _spriteRenderer.sprite = offBlackSprite;
        color.a = offAlpha;
        _spriteRenderer.color = color;
    }
}
