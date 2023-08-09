using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBottle : MonoBehaviour
{
    [SerializeField] Liquid liquid;
    [SerializeField] float max;
    [SerializeField] float min;

    public IHasIndicator indicator;
    private float drainVel;
    void Update()
    {
        if (indicator == null) return;
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, Mathf.Lerp(max, min, indicator.Value), ref drainVel, 0.5f);
    }
}
