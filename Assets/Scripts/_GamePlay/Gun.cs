using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem psToShoot;

    public float fireRate;
    public float maxFireRate = 20;

    public float fireRange;
    float elapsedTime;
    Quaternion defaultRot;
    PaintSection[] paintSections;

    private IEnumerator Start()
    {
        defaultRot = transform.rotation;
        yield return null;
        paintSections = FindObjectsOfType<PaintSection>();
    }
    public void Tick()
    {
        if (!gameObject.activeInHierarchy) return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > (10f / fireRate))
        {
            Shoot();
            elapsedTime = 0;
        }
    }
    public void Shoot()
    {
        var emitParams = new ParticleSystem.EmitParams();
        var nearestFrontPaintSection = paintSections
        .Where(e => e.transform.position.z > transform.position.z)
        .OrderBy(e => (e.transform.position - transform.position).magnitude)
        .FirstOrDefault();
        float dist = float.MaxValue;
        if (nearestFrontPaintSection)
        {
            dist = Vector3.Distance(nearestFrontPaintSection.startPos - Vector3.forward * 1.87f, transform.position);
        }

        emitParams.startLifetime = Mathf.Min(fireRange, dist) / psToShoot.main.startSpeed.constant;

        psToShoot.Emit(emitParams, 1);

        Recoil();
    }

    private void Recoil()
    {
        float time = (10f / fireRate) * 0.99f;
        Quaternion startRot = defaultRot;
        transform.DORotateQuaternion(Quaternion.Euler(Mathf.Lerp(-30, 0, fireRate / maxFireRate), 0, 0) * startRot, time / 3f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() => transform.DORotateQuaternion(startRot, 2 * time / 3f));

    }
}
