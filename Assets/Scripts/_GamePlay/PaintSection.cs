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

    public event System.Action OnTankEmpty;
    private float fillVel;
    private float uiFillVel;
    public bool isPainting { get; set; }

    public Car car { get; private set; }
    public static event System.Action<PaintSection> OnPlayerEnterPaintSection;
    bool rotated;

    public int QueuedNum { get; set; }
    void Start()
    {
        leftTurnTrigger.onTriggerStay += HandleLeftTurn;
        rightTurnTrigger.onTriggerStay += HandleRightTurn;
        triggerGate.onTriggerEnter += HandleTriggerEnter;

        drainSpeed = GameManager.Instance.gameData.inkDrainSpeed;

        Player.Instance.OnGoPastLastBottle += PourTank;
    }

    private void PourTank()
    {
        if (tank.Amount > 0.1f)
        {
            StartCoroutine(WaitForQueue());
        }
    }

    IEnumerator WaitForQueue()
    {
        yield return new WaitUntil(() => QueuedNum == 0);
        var seq = DOTween.Sequence();
        seq.Append(tank.transform.DOMove(sprayGunProp.transform.position + Vector3.up*2.9f + Vector3.left*2.1f, 0.5f));
        seq.Insert(0.255f, tank.transform.DORotate(new Vector3(90, 90, 0),0.5f));

        var tankStartPos = tank.transform.position;
        seq.onComplete += () =>
        {
            tank.Drain();
            GetComponentInChildren<SprayBottle>().startedPouring = true;
            var sequence = DOTween.Sequence();
            sequence.Insert(0.5f, tank.transform.DOJump(tankStartPos, 2, 1, 0.25f));
            sequence.Insert(0.5f, tank.transform.DORotate(Vector3.zero, 0.25f));

        };
    }

    private void HandleTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            painterMovementData.moveSpeed = (endPos - startPos).magnitude / (TankAmount / drainSpeed);
            OnPlayerEnterPaintSection?.Invoke(this);
            GetComponentInChildren<SprayBottle>().startedPouring = false;
        }
    }

    void Update()
    {
        if (!isPainting)
        {
            return;
        }

        if (sprayGunProp.activeInHierarchy) sprayGunProp.SetActive(false);
        tank.Amount -= drainSpeed * Time.deltaTime;

        if (TankAmount <= 0)
        {
            OnTankEmpty?.Invoke();
        }
    }
    public void AddCar(GameObject car, bool secondTime = false)
    {
        float offset = 0;
        car.transform.position = baseTf.transform.position + new Vector3(0, offset, 0);
        car.transform.parent = baseTf;
        if (!secondTime)
            car.transform.forward = baseTf.forward;
        this.car = car.GetComponent<Car>();
    }
    public void DisableTrigger()
    {
        triggerGate.gameObject.SetActive(false);
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
