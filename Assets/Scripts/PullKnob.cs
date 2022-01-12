using UnityEngine;
using System;

public class PullKnob : MonoBehaviour
{
    [SerializeField] private float range = 1;
    [SerializeField] private Transform player;
    private GameObject arrow;
    private float _ropeLength;
    private bool pulling;
    private AudioSource _audioSource;

    public static Action<Vector2> OnPull = delegate { };

    private void Awake() {
        arrow = transform.GetChild(0).gameObject;
        arrow.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _ropeLength = player.gameObject.GetComponent<PathDrawer>().ropeLength;
        OnPull += ActivatePulling;
        Dash.OnDash += SetPulling;
    }
    private void OnDestroy() {
        OnPull -= ActivatePulling;
        Dash.OnDash -= SetPulling;
    }

    private void Update()
    {
        if (InRange() && !pulling)
        {
            if (!arrow.activeSelf)
                arrow.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                _audioSource.Play();
                OnPull(transform.position);
                arrow.SetActive(false);
            }
        }
        else if (arrow.activeSelf)
            arrow.SetActive(false);
    }

    private bool InRange()
    {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).magnitude < range &&
                ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)player.position).magnitude < _ropeLength - 1;
    }
    private void ActivatePulling(Vector2 point) =>
        pulling = true;

    private void SetPulling(Vector2 point) =>
        pulling = point != NullVector.empty;
    
}
