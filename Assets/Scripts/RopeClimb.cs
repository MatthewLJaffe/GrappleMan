using UnityEngine;

/// <summary>
/// allows player to move up and down rope grappled to
/// </summary>
public class RopeClimb : PlayerState
{
    [SerializeField] private float climbSpeed = 1;
    private Grapple _grapple;
    private PathDrawer _pathDrawer;
    private Vector2 tetherPoint = NullVector.empty;
    private Rigidbody2D rb;

    protected override void Awake() {
        _grapple = GetComponent<Grapple>();
        _pathDrawer = GetComponent<PathDrawer>();
        PathDrawer.OnTether += SetTetherPoint;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnDestroy() {
        PathDrawer.OnTether -= SetTetherPoint;
    }

    private void SetTetherPoint(Vector2 tp) =>
        tetherPoint = tp;

    private void FixedUpdate()
    {
        if(Input.GetAxis("Vertical") != 0 && tetherPoint != NullVector.empty)
            ClimbRope();
    }

    public void ClimbRope()
    {
        float displacment = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
        if ((new Vector2(rb.position.x, rb.position.y + displacment) - tetherPoint).magnitude < _pathDrawer.ropeLength + 1)
        {
            rb.position += new Vector2(0, displacment);
            _grapple.PlaceRope();
        }
    }

    public override void Enable() {
        throw new System.Exception("Enable Grapple instead");
    }
}
