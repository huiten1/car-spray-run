using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SprayGun : MonoBehaviour
{

    [SerializeField] float maxHeight = 3;
    [SerializeField] float minHeight = 0;
    [SerializeField] float maxTilt = 30;
    [SerializeField] float minTilt = 0;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] SprayBottle sprayBottle;
    Quaternion minRot;
    Quaternion maxRot;
    private void Start()
    {
        minRot = Quaternion.Euler(minTilt, 0, 0) * transform.rotation;
        maxRot = Quaternion.Euler(maxTilt, 0, 0) * transform.rotation;

    }
    void OnEnable()
    {
        var nearestTank = FindObjectsOfType<InkTank>().OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        progressBar.SetTrackingObject(nearestTank.gameObject);
        sprayBottle.indicator = nearestTank;
    }
    private void Update()
    {
        transform.rotation = Quaternion.Slerp(minRot, maxRot, Mathf.InverseLerp(minHeight, maxHeight, transform.position.y));
    }
}
