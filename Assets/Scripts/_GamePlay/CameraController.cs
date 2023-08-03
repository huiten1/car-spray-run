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
    void Start()
    {
        runnerOffset = followTf.position - transform.position;
        offset = runnerOffset;
        painterOffset = new Vector3(0, -2, 5);
        GameManager.Instance.onPhaseChange += HandlePhaseChange;
    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {
        if (phase == GameManager.GamePhase.Runner)
        {
            offset = runnerOffset;
        }
        if (phase == GameManager.GamePhase.Paint)
        {
            offset = painterOffset;
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
                break;
            default:
                break;
        }
    }
}
