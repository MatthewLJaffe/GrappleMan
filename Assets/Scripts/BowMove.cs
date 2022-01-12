using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMove : ArrowMove
{
    private bool flipY = false;
    protected override void Reorient()
    {
        if (!(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > startPoint.position.x) && !flipY) {
            transform.Rotate(new Vector3(180, 0, 0));
            flipY = true;
        }
        else if(flipY && (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > startPoint.position.x)) {
            transform.Rotate(new Vector3(-180, 0, 0));
            flipY = false;
        }
    }
}
