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
    bool isPainting;
    void Start()
    {
        runnerOffset = followTf.position - transform.position;
        offset = runnerOffset;
        painterOffset = new Vector3(0.0f, -3.5f, 3) * 0.8f;
        painterRotation = Quaternion.Euler(25, -0, 0);
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
        isPainting = phase == GameManager.GamePhase.Paint;
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
                if (isPainting)
                {
                    var targetRot = Quaternion.Euler(currentRotation.eulerAngles.x, Mathf.Lerp(30, -20, (followTf.position.x + 3.5f) / 7f), 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 5);
                }
                break;
            default:
                break;
        }
    }
}
