using UnityEngine;

public class ProjectileShoot : PlayerState
{
    [SerializeField] private float slowFactor;
    [SerializeField] private float resolutionTime;
    [SerializeField] private float maxDrawTime = 1.5f;
    [SerializeField] private float minDrawMultiplier = .5f;
    [SerializeField] private float fastDrawTimeFactor = 6;
    [SerializeField] private GameObject drawPoint;
    [SerializeField] private GameObject projectileArrow;
    private Animator _animator;
    private StaminaDeplete _staminaDeplete;
    private LateralMove _lateralMove;
    private bool staminaEmpty = false;
    private bool slowDown = false;
    private bool drawn = false;
    private float drawTime = 0;
    [SerializeField] private AudioClip arrowShootSound;
    private AudioSource _audioSource;
    [SerializeField] private AudioSource slowMoSource;

    protected override void Awake() {
        base.Awake();
        _animator = GetComponent<Animator>();
        _staminaDeplete = GetComponent<StaminaDeplete>();
        _lateralMove = GetComponent<LateralMove>();
        _audioSource = GetComponent<AudioSource>();
        StaminaDeplete.OnStaminaEmpty += SetStaminaEmpty;
        PullKnob.OnPull += ChangeEnabled;
    }
    private void OnDestroy()
    {
        StaminaDeplete.OnStaminaEmpty -= SetStaminaEmpty;
        PullKnob.OnPull -= ChangeEnabled;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            if(!staminaEmpty && !drawn)
            {
                Draw();
            }
            IncreaseDrawTime();
        }
        if(Input.GetMouseButtonUp(1)) {
            Fire();
        }
        if (Jump.grounded)
            slowDown = false;
        if (Time.timeScale != 1 && !slowDown) {
            CommenceSpeedUp();
        }
    }

    private void Draw(){
        _animator.SetTrigger("Draw");
        drawn = true;
        Enable();
        if (!Jump.grounded)
        {
            CommenceSlowDown();
            _staminaDeplete.SetRun(true);
        }
        drawTime = 0;
    }
    private void IncreaseDrawTime() {
        if (Jump.grounded)
            drawTime += Time.unscaledDeltaTime * fastDrawTimeFactor;
        else
            drawTime += Time.unscaledDeltaTime * fastDrawTimeFactor / 3;
    }

    private void CommenceSlowDown()
    {
        slowMoSource.mute = false;
        slowMoSource.Play();
        slowDown = true;
        Time.timeScale = 1f / slowFactor;
        Time.fixedDeltaTime = 1f / slowFactor * .02f;
    }

    private void CommenceSpeedUp()
    {
        slowMoSource.mute = true;
        Time.timeScale += 1 / resolutionTime * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void Fire()
    {
        if (!drawn) return;
        drawTime = Mathf.Clamp(drawTime, 0, maxDrawTime);
        _audioSource.clip = arrowShootSound;
        _audioSource.volume = Mathf.Clamp(drawTime / maxDrawTime + .1f, 0 ,1);
        _audioSource.Play();
        _lateralMove.Enable();
        drawn = false;
        _animator.SetTrigger("Fire");
        slowDown = false;
        _staminaDeplete.SetRun(false);
        var Arrow = Instantiate(projectileArrow, drawPoint.transform.position, Quaternion.identity);
        Arrow.GetComponent<Projectile>().drawMultiplier = drawTime + minDrawMultiplier;
    }

    private void SetStaminaEmpty(bool b) {
        staminaEmpty = b;
        if(staminaEmpty)
            Fire();
    }

    private void ChangeEnabled(Vector2 point) {
        enabled = point == NullVector.empty;
    }
}
