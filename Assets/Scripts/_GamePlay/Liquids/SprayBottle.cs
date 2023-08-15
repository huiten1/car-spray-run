using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBottle : MonoBehaviour,IHasIndicator
{
    [SerializeField] Liquid liquid;
    [SerializeField] GameObject indicatorObj;
    public IHasIndicator indicator;
    private float drainVel;

    public bool startedPouring { get; set; }
    void Start()
    {
        Liquid.SetLiquidColor(liquid);
        if (indicatorObj)
        {
            indicator = indicatorObj.GetComponent<IHasIndicator>();
        }
    }
    public void SetIndicator(IHasIndicator indicator)
    {
        this.indicator = indicator;
        // Debug.Log("indicator set");
        liquid.FillAmount = indicator.Value;
        startedPouring = true;
    }
    void Update()
    {
        if (indicator == null) return;
        if(!startedPouring) return;
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, indicator.Value, ref drainVel, 0.5f);
    }

    public float Value => indicator == null ? 0 : indicator.Value;
    public Action OnValueChange { get; set; }
}
