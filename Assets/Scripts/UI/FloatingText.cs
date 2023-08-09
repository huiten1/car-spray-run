using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    public string Text { get => text.text; set => text.SetText(value); }
    public Vector3 targetOffset;
    public float duration = 1f;
    void Start()
    {
        transform.DOMove(transform.position + targetOffset, duration);
    }
}
