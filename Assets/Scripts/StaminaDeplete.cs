using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDeplete : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float grappleTime;
    [SerializeField] private float activateCost = 0;
    private float timeLeft;
    private bool run = false;

    public static Action<bool> OnStaminaEmpty = delegate { };

    private void Awake()
    {
        timeLeft = grappleTime;
        slider.maxValue = grappleTime;
        slider.value = grappleTime;
        PathDrawer.OnTether += SetRun;
    }

    private void OnDisable()
    {
        PathDrawer.OnTether -= SetRun;
    }

    private void Update()
    {
        if (run)
        {
            timeLeft -= Time.unscaledDeltaTime;
            slider.value = timeLeft;
            if (timeLeft <= 0) {
                PathDrawer.OnTether(NullVector.empty);
                run = false;
                OnStaminaEmpty(true);
            }
        }
        else if (Jump.grounded && timeLeft < grappleTime)
        {
            timeLeft = grappleTime;
            slider.value = grappleTime;
            OnStaminaEmpty(false);
        }
    }

    private void SetRun(Vector2 tp)
    {
        if (tp == NullVector.empty) {
            run = false;
            timeLeft -= activateCost;
        }
        else
            run = true;
    }

    public void SetRun(bool b)
    {
        run = b;
        if (run)
            timeLeft -= activateCost;
    }
}
