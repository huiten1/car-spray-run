using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObserver : MonoBehaviour
{
    public event System.Action<Collider> onTriggerEnter;
    public event System.Action<Collider> onTriggerStay;
    public event System.Action<Collider> onTriggerExit;

    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
    void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
    void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
}
