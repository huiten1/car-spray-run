using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum FollowMode
    {
        Fixed,
        SmoothDamp
    }
    [SerializeField] FollowMode followMode;

    bool isSmoothTime => followMode == FollowMode.SmoothDamp;
    [NaughtyAttributes.ShowIf("isSmoothTime")]
    [SerializeField] float smoothTime = 0.5f;
    [Space]
    [SerializeField] Transform followTf;
    private Vector3 offset;
    private Vector3 refVel;

    private Vector3 runnerOffset;
    private Vector3 painterOffset;
    private Quaternion painterRotation;
    private Quaternion runnerRotation;
    private Quaternion currentRotation;
    void Start()
    {
        runnerOffset = followTf.position - transform.position;
        offset = runnerOffset;
        painterOffset = new Vector3(0.2f, -4, 6);
        painterRotation = Quaternion.Euler(15, -0, 0);
        GameManager.Instance.onPhaseChange += HandlePhaseChange;
        runnerRotation = transform.rotation;
        currentRotation = runnerRotation;
    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        if (phase == GameManager.GamePhase.Runner)
        {
            offset = runnerOffset;
            currentRotation = runnerRotation;
        }
        if (phase == GameManager.GamePhase.Paint)
        {
            offset = painterRotation * painterOffset;
            currentRotation = painterRotation;
        }
    }

    private void LateUpdate()
    {
        var target = followTf.position - offset;

        switch (followMode)
        {
            case FollowMode.Fixed:
                transform.position = target;
                break;
            case FollowMode.SmoothDamp:
                transform.position = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 3);
                break;
            default:
                break;
        }
    }
}
