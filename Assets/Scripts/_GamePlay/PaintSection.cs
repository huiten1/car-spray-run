using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PaintSection : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float drainSpeed = 0.2f;
    [SerializeField] float turnSpeed = 20;
    public float TankAmount { get => tank.Amount; set => tank.Amount = value; }
    [Header("References")]
    [SerializeField] InkTank tank;
    // [SerializeField] Transform tank;
    [SerializeField] Transform baseTf;
    [Space]
    [SerializeField] TriggerObserver leftTurnTrigger;
    [SerializeField] TriggerObserver rightTurnTrigger;
    [Space]
    [SerializeField] Transform startTF;
    [SerializeField] Transform endTF;
    [Space]
    [SerializeField] GameObject sprayGunProp;

    [Space]
    [SerializeField] TriggerObserver triggerGate;
    [SerializeField] MovementData painterMovementData;

    public Vector3 tankPos => tank.transform.position;
    public Vector3 startPos => startTF.position;
    public Vector3 endPos => endTF.position;

    public event System.Action onTankEmpty;
    private float fillVel;
    private float uiFillVel;
    public bool isPainting { get; set; }

    public Car car { get; private set; }
    public static event System.Action<PaintSection> OnPlayerEnterPaintSection;
    bool rotated;
    void Start()
    {
        leftTurnTrigger.onTriggerStay += HandleLeftTurn;
        rightTurnTrigger.onTriggerStay += HandleRightTurn;
        triggerGate.onTriggerEnter += HandleTriggerEnter;
    }

    private void HandleTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            painterMovementData.moveSpeed = (endPos - startPos).magnitude / (TankAmount / drainSpeed);
            OnPlayerEnterPaintSection?.Invoke(this);
        }
    }

    void Update()
    {
        if (isPainting)
        {
            if (sprayGunProp.activeInHierarchy) sprayGunProp.SetActive(false);
            tank.Amount -= drainSpeed * Time.deltaTime;

            if (TankAmount <= 0)
            {
                onTankEmpty?.Invoke();
            }
        }
    }
    public void AddCar(GameObject car)
    {
        car.transform.position = baseTf.transform.position + Vector3.up * 1.32f;
        car.transform.parent = baseTf;
        car.transform.forward = baseTf.forward;
        this.car = car.GetComponent<Car>();
        // if (rotated) return;
        // var paintPercentage = car.GetComponent<PaintPercentageIndicator>();
        // paintPercentage.OnValueChange += Rotate;

        // void Rotate()
        // {
        //     if (paintPercentage.Value > 0.5f)
        //     {
        //         rotated = true;
        //         paintPercentage.OnValueChange -= Rotate;
        //         baseTf.DORotateQuaternion(Quaternion.Euler(0, 180, 0) * baseTf.rotation, 1f);
        //     }
        // }
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
