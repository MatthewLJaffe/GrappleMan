using UnityEngine;

public class SurfaceChange : SwitchOnOff
{
    private BoxCollider2D boxCollider;

    protected override void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        base.Awake();
    }

    protected override void Activate() {
        boxCollider.enabled = true;
    }

    protected override void Deactivate() {
        boxCollider.enabled = false;
    }
}
