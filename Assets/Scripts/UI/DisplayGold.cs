using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGold : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    private void Update()
    {
        text.SetText($"{GameManager.Instance.Gold}$");
    }
}
