using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

public class PaintPercentageIndicator : MonoBehaviour, IHasIndicator
{
    [SerializeField] float ratioScale;
    public float Value => ratio;
    public Action OnValueChange { get; set; }
    [SerializeField] P3dChangeCounter changeCounter;
    float ratio;
    float t;
    float interval;
    void Start()
    {
        interval = changeCounter.Interval;
    }

    void Update()
    {
        if (Time.time - t > interval)
        {
            ratio = changeCounter.Ratio * ratioScale;
            OnValueChange?.Invoke();
            t = Time.time;
        }
    }
}
