using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] Transform sawTf;

    void Update()
    {
        sawTf.rotation = Quaternion.Euler(0, 0, 500 * Time.deltaTime) * sawTf.rotation;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Movement>().KnockBack(20);
        }
    }
}
