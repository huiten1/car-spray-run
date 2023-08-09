using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBottle : MonoBehaviour, IInteractable, IHasIndicator
{
    public int hitsToBreak = 1;
    [SerializeField] Liquid liquid;
    [SerializeField] float maxAmount = 1.4f;
    [SerializeField] Cash cash;
    public float Value => hitsToBreak;
    public Action OnValueChange { get; set; }
    private int maxHP;

    float fillVel;
    void Start()
    {
        maxHP = hitsToBreak;
    }
    public void Interact(GameObject interactor)
    {
        TakeHit();
    }

    private void TakeHit()
    {
        hitsToBreak -= 1;
        OnValueChange?.Invoke();
        if (hitsToBreak <= 0)
        {
            cash.transform.parent = null;
            cash.gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject);
        }
    }
    void Update()
    {
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, Mathf.Lerp(0, maxAmount, (float)hitsToBreak / maxHP), ref fillVel, 0.2f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LevelComplete();
        }
    }
}
