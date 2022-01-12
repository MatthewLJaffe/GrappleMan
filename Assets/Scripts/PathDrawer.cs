using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathDrawer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Animator _animator;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Material ropeOutline;
    [SerializeField] private Material rope;
    [SerializeField] private float _ropeLength = 10;
    [SerializeField] private int resolution, waveCount, wobbleCount;
    [SerializeField] private float waveSize, animSpeed;
    public float ropeLength { get { return _ropeLength; } private set { _ropeLength = value; } }
    [SerializeField] private LayerMask attachable = 0;
    [SerializeField] private float forgiveRange = 1;
    public static Action<Vector2> OnTether = delegate { };
    private Vector2 tetherPoint = NullVector.empty;
    private Vector2 mousePos;
    private bool pulling = false;
    private GameObject arrowInstance;

    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
        _animator = GetComponentInChildren<Animator>();
        OnTether += SetTetherPoint;
        PullKnob.OnPull += StartPullAnimation;
        arrowInstance = Instantiate(arrow, shootPoint.position, Quaternion.Euler(0, 0, 0));
        arrowInstance.SetActive(false);
    }
    private void OnDestroy() {
        OnTether -= SetTetherPoint;
        PullKnob.OnPull -= StartPullAnimation;
    }     

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && InTetherRange())
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var attachCollider = Physics2D.OverlapPoint(mousePos, attachable);
            if (attachCollider) {
                tetherPoint = mousePos;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, mousePos - (Vector2)shootPoint.position, ropeLength, attachable);
                if (hit && (hit.point - mousePos).magnitude <= forgiveRange) {
                    tetherPoint = hit.point;
                }
            }
            if(tetherPoint != NullVector.empty && !pulling)
            {
                _animator.SetTrigger("Shoot");
                OnTether(tetherPoint);
            }
        }
        else if (Input.GetMouseButtonUp(0) || (Jump.grounded && !pulling))
        {
            if(tetherPoint != NullVector.empty) {
                tetherPoint = NullVector.empty;
                OnTether(NullVector.empty);
                arrowInstance.SetActive(false);
            }
        }

        if (tetherPoint != NullVector.empty) {
            _lineRenderer.SetPosition(0, shootPoint.position);
        }
        else if (InDrawRange())
        {
            _lineRenderer.enabled = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_lineRenderer.material != ropeOutline)
                _lineRenderer.material = ropeOutline;
            _lineRenderer.SetPosition(1, mousePos);
            _lineRenderer.SetPosition(0, shootPoint.position);
        }
        else {
            _lineRenderer.enabled = false;
        }
    }

    public void ShootAnimation(string s) {
        if (s.Equals("Start"))
            StartCoroutine(AnimateRope(tetherPoint));
    }

    private void StartPullAnimation(Vector2 anchor)
    {
        if(anchor != NullVector.empty) {
            tetherPoint = anchor;
            pulling = true;
            _animator.SetTrigger("Shoot");
        }
        else {
            pulling = false;
        }
    }

    private void SetTetherPoint(Vector2 tp) {
        tetherPoint = tp;
    }

    private bool InTetherRange() {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)shootPoint.position).magnitude <= ropeLength;
    }

    private bool InDrawRange() {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)shootPoint.position).magnitude <= ropeLength -1;
    }

    private IEnumerator AnimateRope(Vector3 targetPos)
    {
        if ((Vector2)targetPos == NullVector.empty || _lineRenderer.positionCount == resolution)
            yield break;
        _lineRenderer.enabled = true;
        _lineRenderer.material = rope;
        arrowInstance.SetActive(true);
        arrowInstance.transform.position = shootPoint.position;
        _lineRenderer.positionCount = resolution;
        float percent = 0;
        while(percent < 1f)
        {
            arrowInstance.transform.rotation = Quaternion.Euler(0, 0, LookAtAngle((Vector2)targetPos - (Vector2)shootPoint.position));
            percent += Time.deltaTime * animSpeed;
            SetPoints(targetPos, percent);
            yield return null;
        }
        SetPoints(targetPos, 1);
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(1, targetPos);
        if (pulling)
            tetherPoint = NullVector.empty;
        pulling = false;
    }

    private void SetPoints(Vector2 targetPos, float percent)
    {
        Vector2 ropeEnd = Vector2.Lerp(shootPoint.position, targetPos, percent);
        arrowInstance.transform.position = ropeEnd;
        float length = Vector2.Distance(shootPoint.position, ropeEnd);

        for(int i = 0; i < resolution; i++)
        {
            float xPos = (float)i / resolution * length;
            float reversePercent = (1 - percent);
            //amplitude changes with respect to time
            float amplitude = Mathf.Sin(reversePercent * wobbleCount * Mathf.PI) * waveSize * (1f - (float) i / resolution);
            //ypositions are going to start out wavy and then varry less as we approach reversePrecent = 0
            float yPos = Mathf.Sin((float)waveCount * i / resolution * 2*Mathf.PI * reversePercent) * amplitude;
            Vector2 pos = RotatePoint(new Vector2(shootPoint.position.x + xPos, shootPoint.position.y + yPos), shootPoint.position, LookAtAngle(targetPos - (Vector2)shootPoint.position));
            _lineRenderer.SetPosition(i, pos);
        }
    }

    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0, 0, angle) * dir;
        return (dir + pivot);
    }

    private float LookAtAngle(Vector2 target) {
        return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    }
}
