using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] int prize = 1000;
    [SerializeField] PaintPercentageIndicator indicator;
    public bool isPainted { get; private set; }
    public event Action onPainted;
    void Start()
    {
        indicator.OnValueChange += CheckIfFullyPainted;
    }

    private void CheckIfFullyPainted()
    {
        if (isPainted) return;
        if (indicator.Value > .99f)
        {
            isPainted = true;

            var prefab = Resources.Load<FloatingText>("UI/FloatingText");
            var floatingText = Instantiate(prefab, transform.position + Vector3.up * 2, Quaternion.identity);
            floatingText.Text = $"+{prize}$";
            onPainted?.Invoke();
            GameManager.Instance.Gold += prize;
        }
    }
}
