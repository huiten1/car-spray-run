using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : Singleton<Player>
{
    public event System.Action<PaintSection> OnEnterPaintSection = delegate { };

    [SerializeField] ParticleSystem inkPs;
    [SerializeField] ParticleSystem sprayPs;

    [SerializeField] GameObject sprayGun;
    [SerializeField] GameObject inkGun;
    public float fireRate;
    public float fireRange;

    Dictionary<Gate.Operation, Func<float, float, float>> operationDict = new Dictionary<Gate.Operation, Func<float, float, float>>()
    {
        {Gate.Operation.Add, (x,y)=> x+y},
        {Gate.Operation.Mul, (x,y)=> x*y}
    };

    private void Start()
    {
        Gate.OnEnterGate += HandleEnterGate;
        SetPs(inkPs, 0, 0);
        // SetPs(sprayPs, 0, 0);
        GameManager.Instance.onGameStart += () =>
        {
            UpdatePS(inkPs, 0.1f);
            // UpdatePS(sprayPs, 1.2f);
        };
        GameManager.Instance.onGameStop += () =>
        {
            SetPs(inkPs, 0, 0);
        };

        GameManager.Instance.onPhaseChange += HandlePhaseChange;
    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        if (phase == GameManager.GamePhase.Runner)
        {
            inkGun.SetActive(true);
            sprayGun.SetActive(false);
            inkPs.Play();
            sprayPs.Stop();
        }
        if (phase == GameManager.GamePhase.Paint)
        {
            inkGun.SetActive(false);
            inkPs.Stop();
            sprayPs.Play();
            sprayGun.SetActive(true);
            sprayGun.transform.DOLocalMoveX(1, 1f);
            sprayGun.transform.DORotate(new Vector3(0, -20, 0), 1f);
        }
    }

    private void OnDestroy()
    {
        Gate.OnEnterGate -= HandleEnterGate;
    }
    private void HandleEnterGate(Gate.Operation operation, Gate.TargetField field, float value)
    {
        if (field == Gate.TargetField.FireRange)
        {
            fireRange = operationDict[operation](fireRange, value);
        }
        if (field == Gate.TargetField.FireRate)
        {
            fireRate = operationDict[operation](fireRate, value);
        }
        UpdatePS(inkPs, 0.1f);
        // UpdatePS(sprayPs, 1.2f);
    }
    private void SetPs(ParticleSystem ps, float rate, float range)
    {
        var emission = ps.emission;
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = rate;
        emission.rateOverTime = rateOverTime;

        var main = ps.main;
        var lifeTime = ps.main.startLifetime;
        lifeTime.constant = Mathf.Lerp(0.5f, 1.5f, range / 100f);
        main.startLifetime = lifeTime;
    }
    private void UpdatePS(ParticleSystem ps, float multiplier) => SetPs(ps, fireRate * multiplier, fireRange);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PaintSection"))
        {
            other.gameObject.SetActive(false);
            OnEnterPaintSection?.Invoke(other.GetComponentInParent<PaintSection>());
        }
    }
}
