using System;
using UnityEngine;
/// <summary>
/// vertical jump usable when player is grounded
/// </summary>
public class Jump : PlayerState
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Animator _animator;
    [SerializeField] private float jumpForce;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private AudioClip thudSound;
    public static bool grounded = true;
    private AudioSource _audioSource;
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded) {
            _animator.SetTrigger("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
            
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0 && !grounded)
            rb.gravityScale = fallMultiplier;
        else if (!grounded && !Input.GetButton("Jump") && rb.velocity.y > 0)
            rb.gravityScale = lowJumpMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && collision.otherCollider == circleCollider) {
            _animator.SetBool("Grounded", true);
            grounded = true;
            rb.gravityScale = 1;
            _audioSource.volume = 1f;
            _audioSource.clip = thudSound;
            _audioSource.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && collision.otherCollider == circleCollider) {
            _animator.SetBool("Grounded", false);
            grounded = false;
        }
    }

    public override void Enable() {
        throw new Exception("Enable LateralMove instead");
    }
}
