using UnityEngine;
/// <summary>
/// state for moving player while grappling
/// </summary>
public class GrappleMove : PlayerState
{
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float acceleration = 1;
    private Rigidbody2D rb;

    protected override void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(Input.GetAxis("Horizontal") != 0 && Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 preV = rb.velocity;
            rb.AddForce(Vector2.right * horizontal * Time.fixedDeltaTime * acceleration, ForceMode2D.Impulse);
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = preV;
        }
    }

    public override void Enable() {
        throw new System.Exception("Enable Grapple instead");
    }
}
