using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PaintIn3D;
using UnityEngine;

public class SprayGun : MonoBehaviour
{

    [SerializeField] float maxHeight = 3;
    [SerializeField] float minHeight = 0;
    [SerializeField] float maxTilt = 30;
    [SerializeField] float minTilt = 0;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] SprayBottle sprayBottle;
    [SerializeField] ParticleSystem sprayPs;

    private Color color;
    Quaternion minRot;
    Quaternion maxRot;
    private void Start()
    {
        minRot = Quaternion.Euler(minTilt, 0, 0) * transform.rotation;
        maxRot = Quaternion.Euler(maxTilt, 0, 0) * transform.rotation;
        var main = sprayPs.main;
        main.startColor = GameManager.Instance.gameData.color;
        sprayPs.GetComponent<P3dPaintSphere>().Color = (GameManager.Instance.gameData.currentLevel + 1) % 3 == 0 ? Color.black : GameManager.Instance.gameData.color;
        
    }
    void OnEnable()
    {
        var nearestTank = FindObjectsOfType<InkTank>().OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        progressBar.SetTrackingObject(nearestTank.gameObject);
        sprayBottle.SetIndicator(nearestTank);

    }
    private void Update()
    {
        // transform.rotation = Quaternion.Slerp(minRot, maxRot, Mathf.InverseLerp(minHeight, maxHeight, transform.position.y));
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
