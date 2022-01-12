using UnityEngine;
using System;

public class Grapple : PlayerState
{
    [SerializeField] private GameObject grapple = null;
    [SerializeField] private float gravityMultiplier = 0;
    [SerializeField] private float releaseMultiplier = 0;
    [SerializeField] private AudioClip grappleSound;
    private LateralMove _lateralMove;
    private Vector2 anchorPoint = NullVector.empty;
    private GameObject grappleInstance;
    private Rigidbody2D rb;
    private bool grappled;
    private AudioSource _audioSource;
    
    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        PathDrawer.OnTether += SetAnchorPoint;
        _lateralMove = GetComponent<LateralMove>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnDestroy() {
        PathDrawer.OnTether -= SetAnchorPoint;
    }

    private void Update()
    {
        if(anchorPoint != NullVector.empty)
        {
            if(!grappled && anchorPoint.y > transform.position.y) {
                AttachRope();
                _audioSource.volume = 1f;
                _audioSource.clip = grappleSound;
                _audioSource.Play();
            }
            else if(grappled && anchorPoint.y < transform.position.y) {
                DestroyGrapple();
            }
        }
        else if (grappled) {
            DestroyGrapple();
        }
    }

    private void SetAnchorPoint(Vector2 tp)
    {
        anchorPoint = tp;
    }

    private void AttachRope()
    {
        Enable();
        grappleInstance = Instantiate(grapple);
        PlaceRope();
        HingeJoint2D bottomHinge = grappleInstance.GetComponents<HingeJoint2D>()[1];
        bottomHinge.connectedBody = rb;
        rb.gravityScale = gravityMultiplier;
        grappled = true;
    }

    public void PlaceRope()
    {
        if (grappleInstance)
        {
            Vector2 distance = anchorPoint - rb.position;
            float theta = Mathf.Acos(Vector2.Dot(Vector2.up, distance) / distance.magnitude) * (180 / Mathf.PI);
            float direction = -1;
            if (rb.position.x > anchorPoint.x)
                direction = 1;
            grappleInstance.transform.position = (rb.position + anchorPoint) / 2;
            grappleInstance.transform.rotation = Quaternion.Euler(0, 0, theta * direction);
            grappleInstance.transform.localScale = new Vector3(.2f, distance.magnitude, 1);
        }
    }

    public void DestroyGrapple()
    {
        if (GameObject.FindGameObjectWithTag("Grapple"))
        {
            _lateralMove.Enable();
            Destroy(grappleInstance);
            grappleInstance = null;
            grappled = false;
            rb.AddForce(new Vector2(rb.velocity.x * (releaseMultiplier - 1), rb.velocity.y * (releaseMultiplier - 1)), ForceMode2D.Impulse);
        }
        else
            throw new Exception("trying to destory grapple that doesn't exist");
    }
}
