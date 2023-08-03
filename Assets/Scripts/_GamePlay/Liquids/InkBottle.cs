using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InkBottle : MonoBehaviour, IInteractable
{
    public float currentAmount = 0;
    [SerializeField] Liquid liquid;
    [SerializeField] float maxAmount = 1.4f;
    public bool isEndingBottle;
    void Start()
    {
        liquid.FillAmount = maxAmount;
    }
    public void Interact(GameObject interactor)
    {
        Fill();
    }

    private void Fill()
    {
        currentAmount += 0.1f;
    }
    float fillVel;
    void Update()
    {
        liquid.FillAmount = Mathf.SmoothDamp(liquid.FillAmount, maxAmount - currentAmount, ref fillVel, 0.2f);

        if (currentAmount >= maxAmount)
        {
            if (isEndingBottle)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isEndingBottle)
            {
                GameManager.Instance.LevelComplete();
            }
            else
            {
                GoToTank();
            }
        }
    }

    private void GoToTank()
    {
        var seq = DOTween.Sequence();
        var paintSection = FindObjectsOfType<PaintSection>()
        .Where(e => e.transform.position.z > transform.position.z)
        .OrderBy(e => e.transform.position.z)
        .FirstOrDefault();

        Vector3 tankPos = paintSection.tankPos;
        Vector3 nearestConveyorPos = transform.position;
        nearestConveyorPos.x = FindObjectOfType<SideConveyor>().transform.position.x;
        nearestConveyorPos.z += 5;

        seq.Append(transform.DOMove(nearestConveyorPos, 1f));
        seq.Append(transform.DOMove(tankPos + Vector3.back, Vector3.Distance(tankPos, transform.position) / 10f));
        seq.Append(transform.DOMove(tankPos + new Vector3(0, 2, -1), 1f));
        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(90, 0, 0) * transform.rotation, 1f));


        seq.OnComplete(() =>
        {
            paintSection.tankAmount += currentAmount;
            DOTween.To(() => currentAmount, n => currentAmount = n, 0, 1f).OnComplete(() => Destroy(gameObject));
        });
    }
}
