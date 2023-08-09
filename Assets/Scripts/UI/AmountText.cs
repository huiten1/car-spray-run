using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountText : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] GameObject targetObject;
    [SerializeField] string format;
    IHasIndicator indicator;
    void Start()
    {

        indicator = targetObject.GetComponent<IHasIndicator>();
        text.SetText(String.Format(format, indicator.Value));
        indicator.OnValueChange += () => text.SetText(String.Format(format, indicator.Value));
    }
}
public interface IHasIndicator
{
    public float Value { get; }
    public System.Action OnValueChange { get; set; }
}
