using System.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem psToShoot;
    [SerializeField] Liquid liquid;
    [SerializeField] Renderer handRenderer;
    [SerializeField] Renderer inkGunRenderer;
    [SerializeField] Color feedbackColor;
    public float fireRate;
    public float maxFireRate = 20;

    public float fireRange;
    float elapsedTime;
    Quaternion defaultRot;
    PaintSection[] paintSections;
    bool rested = true;

    private List<SprayBottlePair> inkBottlesZs = new();

    struct SprayBottlePair
    {
        public float InkBottleZ;
        public float PaintSectionZ;
    }
    private IEnumerator Start()
    {
        defaultRot = transform.rotation;
        yield return null;
        paintSections = FindObjectsOfType<PaintSection>();
        psToShoot.GetComponent<ParticleSystemRenderer>().material.color = GameManager.Instance.gameData.color;
        var main = psToShoot.main;
        main.startSpeed = GameManager.Instance.gameData.bulletSpeed;
        var childMain = psToShoot.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        childMain.startColor = GameManager.Instance.gameData.color;

        var grandChildMain = psToShoot.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
        grandChildMain.startColor = GameManager.Instance.gameData.color;

        Liquid.SetLiquidColor(liquid);

        var inkBottles = FindObjectsOfType<InkBottle>();
        foreach (var section in paintSections)
        {
            var z = inkBottles.Where(e => e.transform.position.z < section.startPos.z).Max(e => e.transform.position.z);
            inkBottlesZs.Add(new SprayBottlePair{InkBottleZ = z,PaintSectionZ = section.transform.position.z});
        }
    }
    public void Tick()
    {
        if (!gameObject.activeInHierarchy) return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > (10f / fireRate) && rested)
        {
            Shoot();
            elapsedTime = 0;
        }
    }
    public void Shoot()
    {
        var emitParams = new ParticleSystem.EmitParams();
        var sprayBottlePair = inkBottlesZs.Where(e=>e.PaintSectionZ>transform.position.z).OrderBy(e=>e.InkBottleZ).FirstOrDefault();

        float dist = sprayBottlePair.InkBottleZ!=0? Mathf.Abs(sprayBottlePair.PaintSectionZ-transform.position.z-3) : fireRange;
        emitParams.startLifetime = Mathf.Min(fireRange, dist) / psToShoot.main.startSpeed.constant;
        // if (sprayBottlePair.PaintSectionZ > transform.position.z && sprayBottlePair.InkBottleZ < transform.position.z)
        // {
        //     return;
        // }
        psToShoot.Emit(emitParams, 1);
        Recoil();
    }

    private void Recoil()
    {
        rested = false;
        float time = (10f / fireRate) * 0.99f;
        Quaternion startRot = defaultRot;
        var seq = DOTween.Sequence();
        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(Mathf.Lerp(-30, 0, fireRate / maxFireRate), 0, 0) * startRot, time / 3f)
            .SetEase(Ease.OutExpo));
        seq.Append(transform.DORotateQuaternion(startRot, 2 * time / 3f));
        seq.OnComplete(() => rested = true);

    }

    internal void DamageFeedback()
    {
        Flash(handRenderer, feedbackColor);
        Flash(inkGunRenderer, feedbackColor);
    }

    private void Flash(Renderer renderer, Color color)
    {
        var startColor = renderer.material.color;
        renderer.material.DOColor(color, 0.1f).OnComplete(() => renderer.material.DOColor(startColor, 0.1f));
    }
}
