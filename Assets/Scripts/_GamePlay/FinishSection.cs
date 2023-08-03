using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSection : MonoBehaviour
{
    [SerializeField] InkBottle bottlePrefab;

    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(bottlePrefab, transform.position + new Vector3(-2.5f, 0, i * 2.5f), Quaternion.identity).isEndingBottle = true;
            Instantiate(bottlePrefab, transform.position + new Vector3(0.0f, 0, i * 2.5f), Quaternion.identity).isEndingBottle = true;
            Instantiate(bottlePrefab, transform.position + new Vector3(2.5f, 0, i * 2.5f), Quaternion.identity).isEndingBottle = true;
        }
    }
}
