using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSection : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float turnSpeed = 20;
    [SerializeField] Liquid tankLiquid;
    public float tankAmount = 0;
    [Header("References")]
    [SerializeField] Transform tank;
    [SerializeField] Transform baseTf;
    [Space]
    [SerializeField] TriggerObserver leftTurnTrigger;
    [SerializeField] TriggerObserver rightTurnTrigger;
    [Space]
    [SerializeField] Transform startTF;
    [SerializeField] Transform endTF;

    public Vector3 tankPos => tank.position;
    public Vector3 startPos => startTF.position;
    public Vector3 endPos => endTF.position;

    public event System.Action onTankEmpty;
    private float fillVel;
    public bool isPainting { get; set; }

    void Start()
    {
        leftTurnTrigger.onTriggerStay += HandleLeftTurn;
        rightTurnTrigger.onTriggerStay += HandleRightTurn;

    }
    void Update()
    {
        tankLiquid.FillAmount = Mathf.SmoothDamp(tankLiquid.FillAmount, 1.2f - tankAmount * 0.3f, ref fillVel, 0.5f);
        if (isPainting)
        {
            tankAmount -= 0.2f * Time.deltaTime;

            if (tankAmount <= 0)
            {
                onTankEmpty?.Invoke();
            }
        }
    }
    public void AddCar(GameObject car)
    {
        car.transform.position = baseTf.transform.position + Vector3.up * 1.32f;
        car.transform.parent = baseTf;
    }
    private void HandleRightTurn(Collider collider)
    {
        baseTf.transform.rotation = Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0) * baseTf.transform.rotation;
    }

    private void HandleLeftTurn(Collider collider)
    {
        baseTf.transform.rotation = Quaternion.Euler(0, -turnSpeed * Time.deltaTime, 0) * baseTf.transform.rotation;
    }


}
