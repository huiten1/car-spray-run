using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image progressImage;
    [SerializeField] GameObject objectToTrack;
    public IHasIndicator indicator;
    private float refSpeed;
    private void Start()
    {
        if (objectToTrack)
            SetTrackingObject(objectToTrack);
    }
    public void SetTrackingObject(GameObject gameObject)
    {
        indicator = gameObject.GetComponent<IHasIndicator>();
        // indicator.OnValueChange += HandleValueChange;
        progressImage.fillAmount = indicator.Value;
    }
    void Update()
    {
        progressImage.fillAmount = Mathf.SmoothDamp(progressImage.fillAmount, indicator.Value, ref refSpeed, 0.5f);
    }
    // private void HandleValueChange()
    // {
    //     progressImage.fillAmount = indicator.Value;
    // }
}
