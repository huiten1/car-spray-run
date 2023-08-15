using System;
using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] Operation operation;
    [SerializeField] TargetField targetField;
    [SerializeField] float value;
    [SerializeField] TMPro.TMP_Text fieldNameText;
    [SerializeField] TMPro.TMP_Text valText;

    [SerializeField] Renderer gateRenderer;
    [SerializeField] Renderer[] poleRenderers;
    [SerializeField] Color fireRateColor;
    [SerializeField] Color fireRateGateColor;
    [SerializeField] Color fireRangeColor;
    [SerializeField] Color fireRangeGateColor;
    public static event System.Action<Operation, TargetField, float> OnEnterGate;
    public enum Operation
    {
        Add,
        Mul,
    }
    public enum TargetField
    {
        FireRate,
        FireRange
    }
    static Dictionary<Operation, char> opToChar = new Dictionary<Operation, char>()
    {
        {Operation.Add,'+'},
        {Operation.Mul,'x'}
    };

    void Start()
    {
        // targetField = (TargetField)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TargetField)).Length);
        targetField = UnityEngine.Random.Range(0, 1f) > 0.8f ? TargetField.FireRange : TargetField.FireRate;
        fieldNameText.SetText(targetField.ToString());
        UpdateValText();
        gateRenderer.material.color = targetField == TargetField.FireRate ? fireRateColor : fireRangeColor;
        foreach (var item in poleRenderers)
        {
            item.material.color = targetField == TargetField.FireRate ? fireRateGateColor : fireRangeGateColor;
        }
    }

    void UpdateValText() => valText.SetText($"{opToChar[operation]}{value}");

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnEnterGate?.Invoke(operation, targetField, value);
            Destroy(gameObject);
        }
    }


    public void Interact(GameObject interactor)
    {
        value += 1;
        UpdateValText();
    }
}
