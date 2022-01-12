using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float drawMultiplier = 1;
    [SerializeField] private float speed = 10;
    [SerializeField] private float liveTime = 5;
    private Rigidbody2D rb;
    private Animator _animator;
    private bool rotate = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float theta = GetVectorAngle(dir);
        transform.rotation = Quaternion.Euler(0, 0, theta);
        rb.velocity = dir.normalized * speed;
    }
    private void Start()
    {
        StartCoroutine(FadeProjectile(liveTime));
        rb.velocity *= drawMultiplier;
    }

    private void FixedUpdate()
    {
        if(rotate)
            transform.rotation = Quaternion.Euler(0, 0, GetVectorAngle(rb.velocity));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        rotate = false;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        transform.parent = collision.gameObject.transform;
    }

    private IEnumerator FadeProjectile(float liveTime)
    {
        yield return new WaitForSeconds(liveTime);
        _animator.SetTrigger("Destroy");
    }

    public void DestroyProjectile() {
        Destroy(gameObject);
    }

    private float GetVectorAngle(Vector2 dir) {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }



}
