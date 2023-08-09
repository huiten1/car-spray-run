using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovement : MonoBehaviour
{
    public float span = 3f;
    public float speed = 1f;
    public float cycleOffset = 0f;
    Vector3 left;
    Vector3 right;
    float t = 0;
    void Start()
    {
        left = new Vector3(-span / 2f, 0, transform.position.z);
        right = new Vector3(span / 2f, 0, transform.position.z);
        t = cycleOffset * speed;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(left, right, Mathf.PingPong(t, 1));
        t += Time.deltaTime * speed;
    }
}
