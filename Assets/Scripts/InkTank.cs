using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using DG.Tweening;

public class InkTank : MonoBehaviour, IHasIndicator
{
    public float maxAmount = 1.2f;
    public float Amount
    {
        get => amount;
        set
        {
            amount = value;
            amount = Mathf.Clamp(amount, 0, maxAmount);
            OnValueChange?.Invoke();
        }
    }
    private float amount;
    [SerializeField] Liquid liquid;
    private float fillVel;
    public float Value => Amount / maxAmount;
    public Action OnValueChange { get; set; }
    void Start()
    {
        var pos = transform.position;
        pos.x = FindObjectOfType<SideConveyor>().transform.position.x;
        transform.position = pos;
        Liquid.SetLiquidColor(liquid);
    }

    private bool isDraining;
    public void Drain()
    {
        isDraining = true;
        DOTween.To(() => liquid.FillAmount, x => liquid.FillAmount=x, 0,0.5f);
    }
    void Update()
    {
        if(isDraining) return;
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, amount / maxAmount, ref fillVel, 0.5f);
    }
}
