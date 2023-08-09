using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class InkTank : MonoBehaviour, IHasIndicator
{
    public float maxAmount = 1.2f;

    public float Amount
    {
        get => amount;
        set
        {
            amount = value;
            // amount = Mathf.Clamp(amount, 0, maxAmount);
            OnValueChange?.Invoke();
        }
    }
    private float amount;
    [SerializeField] Liquid liquid;
    private float fillVel;
    public float Value => Amount * .3f / maxAmount;
    public Action OnValueChange { get; set; }
    void Start()
    {
        var pos = transform.position;
        pos.x = FindObjectOfType<SideConveyor>().transform.position.x;
        transform.position = pos;
    }
    void Update()
    {
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, maxAmount - Amount * 0.3f, ref fillVel, 0.5f);
    }
}
