using System;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float strafeSpeed;
    Vector3 lastMP;
    private float currentSpeed;
    IMovement currentMovement;
    float startY;
    void Start()
    {
        currentSpeed = speed;
        startY = transform.position.y;
        currentMovement = new RunnerMovement(transform);
        GameManager.Instance.onPhaseChange += HandlePhaseChange;
    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        switch (phase)
        {
            case GameManager.GamePhase.Runner:
                currentMovement = new RunnerMovement(transform);
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                break;
            case GameManager.GamePhase.Paint:
                currentMovement = new PainterMovement(transform);
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
    public RunnerMovement(Transform transform)
    {
        this.transform = transform;
    }
    public void Tick(Vector3 input)
    {
        transform.position += new Vector3(input.x, 0, input.z) * Time.deltaTime;
    }
}
public class PainterMovement : IMovement
{
    public Transform transform;
    public PainterMovement(Transform transform)
    {
        this.transform = transform;
    }
    public void Tick(Vector3 input)
    {
        transform.position += new Vector3(input.x, input.y, 0) * Time.deltaTime;
    }
}
