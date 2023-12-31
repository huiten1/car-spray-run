using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : Singleton<Player>
{
    public event System.Action<PaintSection> OnEnterPaintSection;

    public event System.Action OnGoPastLastBottle;
    // [SerializeField] ParticleSystem inkPs;
    [SerializeField] ParticleSystem sprayPs;

    [SerializeField] GameObject sprayGun;
    // [SerializeField] GameObject inkGun;
    [SerializeField] Gun inkGun;
    public float fireRate;
    public float fireRange;
    
    readonly Dictionary<Gate.Operation, Func<float, float, float>> operationDict = new()
    {
        {Gate.Operation.Add, (x,y)=> x+y},
        {Gate.Operation.Mul, (x,y)=> x*y}
    };

    private void Start()
    {
        Gate.OnEnterGate += HandleEnterGate;
        GameManager.Instance.onPhaseChange += HandlePhaseChange;
    }
    private void Update()
    {
        if (!GameManager.Instance.isPlaying) return;

        inkGun.Tick();
    }
    private void OnDestroy()
    {
        Gate.OnEnterGate -= HandleEnterGate;
    }
    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        if (phase == GameManager.GamePhase.Runner)
        {
            inkGun.gameObject.SetActive(true);
            sprayGun.SetActive(false);

            sprayPs.Stop();
        }
        if (phase == GameManager.GamePhase.Paint)
        {
            inkGun.gameObject.SetActive(false);
            sprayGun.SetActive(true);
            var targetLocalPos = sprayGun.transform.localPosition;
            sprayGun.transform.localPosition = inkGun.transform.localPosition;
            sprayGun.transform.DOLocalMove(targetLocalPos, 1f).onComplete+=sprayPs.Play;

            // sprayGun.transform.DORotate(new Vector3(0, -90, 0), 1f);
        }
    }
    private void HandleEnterGate(Gate.Operation operation, Gate.TargetField field, float value)
    {
        if (field == Gate.TargetField.FireRange)
        {
            inkGun.fireRange = operationDict[operation](inkGun.fireRange, value);
        }
        if (field == Gate.TargetField.FireRate)
        {
            inkGun.fireRate = operationDict[operation](inkGun.fireRate, value);
        }
    }
    public void DamageFeedback()
    {
        inkGun.DamageFeedback();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LastBottle"))
        {
            OnGoPastLastBottle?.Invoke();
        }
    }
}
