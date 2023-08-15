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
    public float fontSize = 72f;
    public DissapearType dissapearType;
    public enum DissapearType
    {
        Fade,
        None
    }
    void Start()
    {
        text.fontSize = fontSize;
        switch (dissapearType)
        {
            case DissapearType.Fade:
                text.DOFade(0, duration);
                break;
            default:
                break;
        }
        transform.DOMove(transform.position + targetOffset, duration).SetEase(Ease.OutQuad);
    }
}
