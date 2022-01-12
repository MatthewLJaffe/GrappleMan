using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    [SerializeField] protected Transform startPoint;
    private float prevTheta = 0;


    private void Update() {
        float theta = CalculateAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPoint.position);
        transform.RotateAround(transform.parent.position, Vector3.forward, theta - prevTheta);
        Reorient();
        prevTheta = theta;
    }

    private float CalculateAngle(Vector2 dir) {
        return Mathf.Acos(Vector2.Dot(Vector2.up, dir) / dir.magnitude) * Mathf.Rad2Deg * Direction();
    }

    protected virtual void Reorient() { }

    protected virtual float Direction()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > startPoint.position.x) {
            return -1;
        }
        else {
            return 1;
        }
            
    }
}
