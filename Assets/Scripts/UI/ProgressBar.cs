using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image progressImage;
    [SerializeField] GameObject objectToTrack;
    [SerializeField] TMP_Text progressText;
    public IHasIndicator indicator;
    private float refSpeed;
    public bool smoothDamp;
    private void Start()
    {
        if (objectToTrack)
            SetTrackingObject(objectToTrack);


        progressImage.color = GameManager.Instance.gameData.color;
        Debug.Log($"image color set:{progressImage.color}");
    }
    public void SetTrackingObject(GameObject gameObject)
    {
        indicator = gameObject.GetComponent<IHasIndicator>();
        // indicator.OnValueChange += HandleValueChange;
        progressImage.fillAmount = indicator.Value;
    }
    void Update()
    {
        if (smoothDamp)
        {
        progressImage.fillAmount = Mathf.SmoothDamp(progressImage.fillAmount, indicator.Value, ref refSpeed, 0.5f);
        }
        else
        {
            progressImage.fillAmount = indicator.Value;
        }
        if (progressText)
            progressText.SetText(String.Format("{0:P0}", Mathf.Clamp01(indicator.Value)));
    }
    // private void HandleValueChange()
    // {
    //     progressImage.fillAmount = indicator.Value;
    // }
}
