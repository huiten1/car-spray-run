using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSection : MonoBehaviour
{
    [SerializeField] EndingBottle bottlePrefab;
    [SerializeField]
    float startDistance;
    [SerializeField] float offset = 2.5f;
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(bottlePrefab, transform.position + new Vector3(-2.0f, 0, startDistance + i * offset), Quaternion.identity).hitsToBreak = (i + 1) * 3;
            Instantiate(bottlePrefab, transform.position + new Vector3(+0.0f, 0, startDistance + i * offset), Quaternion.identity).hitsToBreak = (i + 1) * 3;
            Instantiate(bottlePrefab, transform.position + new Vector3(+2.0f, 0, startDistance + i * offset), Quaternion.identity).hitsToBreak = (i + 1) * 3;
        }
    }
}
