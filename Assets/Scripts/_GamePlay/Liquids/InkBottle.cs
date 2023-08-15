using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InkBottle : MonoBehaviour, IInteractable, IHasIndicator
{
    public float currentAmount = 0;
    [SerializeField] Liquid liquid;
    [SerializeField] float maxAmount = 1.4f;

    [SerializeField] ParticleSystem pourVFX;
    [SerializeField] Transform cap;
    [SerializeField] GameObject percentageText;


    public float Value => currentAmount / maxAmount;
    public Action OnValueChange { get; set; }

    void Start()
    {
        Liquid.SetLiquidColor(liquid);
        var main = pourVFX.main;
        main.startColor = GameManager.Instance.gameData.color;
    }



    public void Interact(GameObject interactor)
    {
        Fill();
        Feedback();
    }

    private void Feedback()
    {
        if (DOTween.IsTweening(transform)) return;
        var startScale = transform.localScale.x;
        transform.DOScale(startScale * 1.05f, 0.1f).SetEase(Ease.OutBack).OnComplete(() => transform.DOScale(startScale, 0.1f));
    }

    private void Fill()
    {
        if (currentAmount == 0)
        {
            percentageText.SetActive(true);
        }
        currentAmount += 0.1f;

        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        OnValueChange?.Invoke();
    }
    float fillVel;



    void Update()
    {
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, currentAmount / maxAmount, ref fillVel, 0.2f);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GoToTank();
        }
    }

    private void GoToTank()
    {
        if (GetComponent<SideMovement>())
            GetComponent<SideMovement>().enabled = false;
        var seq = DOTween.Sequence();
        var paintSection = FindObjectsOfType<PaintSection>()
        .Where(e => e.transform.position.z > transform.position.z)
        .OrderBy(e => e.transform.position.z)
        .FirstOrDefault();

        Vector3 tankPos = paintSection.tankPos;
        Vector3 nearestConveyorPos = transform.position;
        nearestConveyorPos.x = FindObjectOfType<SideConveyor>().transform.position.x;
        nearestConveyorPos.z += 5;

        seq.Append(transform.DOMove(nearestConveyorPos, Vector3.Distance(transform.position, nearestConveyorPos) / 25).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(tankPos + Vector3.back, Vector3.Distance(tankPos, transform.position) / 25).SetEase(Ease.Linear));
        seq.Append(transform.DOMove(tankPos + new Vector3(0, 2, -2), 0.1f));

        paintSection.QueuedNum += 1;
        
        seq.OnComplete(() =>
        {
            cap.gameObject.SetActive(false);
            pourVFX.gameObject.SetActive(true);
            pourVFX.emissionRate = 0;
            var duration = .5f;
            DOTween.To(() => pourVFX.emissionRate, val => pourVFX.emissionRate = val, 20, duration).SetEase(Ease.OutSine);
            var tween = transform.DORotateQuaternion(Quaternion.Euler(90, 0, 0) * transform.rotation, duration).SetEase(Ease.OutCubic);

            tween.OnComplete(() =>
            {
                DOTween.To(() => pourVFX.emissionRate, val => pourVFX.emissionRate = val, 0, duration);
                paintSection.TankAmount += currentAmount;
                DOTween.To(() => currentAmount, n => currentAmount = n, 0, duration).OnComplete(() =>
                {
                    paintSection.QueuedNum -= 1;
                    Destroy(gameObject);
                });
               
            });

        });



    }
}
