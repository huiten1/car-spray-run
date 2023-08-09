using System;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float strafeSpeed;
    [SerializeField] MovementData paintMovementData;

    Vector3 lastMP;
    private float currentSpeed;
    IMovement currentMovement;
    float startY;

    void Start()
    {
        currentSpeed = speed;
        startY = transform.position.y;
        float width = GameManager.Instance.gameData.roadWidth - 1f;
        currentMovement = new RunnerMovement(transform) { minX = -width / 2f, maxX = width / 2f };
        GameManager.Instance.onPhaseChange += HandlePhaseChange;

    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        switch (phase)
        {
            case GameManager.GamePhase.Runner:
                float width = GameManager.Instance.gameData.roadWidth - 1f;
                currentMovement = new RunnerMovement(transform) { minX = -width / 2f, maxX = width / 2f };
                transform.position = new Vector3(transform.position.x, startY, transform.position.z + 5);
                break;
            case GameManager.GamePhase.Paint:
                var car = FindObjectsOfType<Car>().OrderBy(e => (transform.position - e.transform.position).sqrMagnitude).FirstOrDefault();
                currentMovement = new PainterMovement(transform) { carBaseTf = car.transform.parent, speed = paintMovementData.moveSpeed };
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPlaying) return;
        var input = new Vector3(0, 0, currentSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            lastMP = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var delta = (Input.mousePosition - lastMP);
            input.x = delta.x * strafeSpeed;
            input.y = delta.y * strafeSpeed;
            lastMP = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            input.x = 0;
            input.y = 0;
        }

        currentMovement.Tick(input);
    }
    public void KnockBack(float amount)
    {
        currentSpeed -= amount;

        DOTween.To(() => currentSpeed, (val) => currentSpeed = val, speed, 0.5f);
    }
    public void Slow(float amount)
    {
        currentSpeed = speed - amount;
    }
    public void Reset()
    {
        currentSpeed = speed;
    }
}
public class RunnerMovement : IMovement
{
    private Transform transform;
    public float minX;
    public float maxX;
    public RunnerMovement(Transform transform)
    {
        this.transform = transform;
    }
    public void Tick(Vector3 input)
    {
        var target = transform.position + new Vector3(input.x, 0, input.z) * Time.deltaTime;
        target.x = Mathf.Clamp(target.x, minX, maxX);
        transform.position = target;
    }
}
public class PainterMovement : IMovement
{
    public Transform carBaseTf;
    public Transform transform;
    public float speed;
    public PainterMovement(Transform transform)
    {
        this.transform = transform;
    }
    public void Tick(Vector3 input)
    {
        // carBaseTf.rotation = Quaternion.Euler(0, -input.x * Time.deltaTime * 20, 0) * carBaseTf.rotation;
        transform.position += new Vector3(input.x, input.y, 0) * speed * Time.deltaTime;
    }
}
