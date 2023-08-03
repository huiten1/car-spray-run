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
        fieldNameText.SetText(targetField.ToString());
        UpdateValText();
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
