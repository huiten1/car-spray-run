using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadMill : MonoBehaviour
{
    [SerializeField] float slowAmount;
    Material currentMaterial;
    float t = 0;
    void Start()
    {
        currentMaterial = GetComponent<Renderer>().material;
    }
    void Update()
    {
        currentMaterial.SetTextureOffset("_MainTex", new Vector2(0, t));
        t += Time.deltaTime;
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Movement>().Slow(slowAmount);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Movement>().Reset();
        }
    }
}
