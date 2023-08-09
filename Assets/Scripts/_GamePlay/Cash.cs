using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    public int amount;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            var prefab = Resources.Load<FloatingText>("UI/FloatingText");
            var floatingText = Instantiate(prefab, transform.position, Quaternion.identity);
            floatingText.Text = $"+{amount}$";
            GameManager.Instance.Gold += amount;
            Destroy(gameObject);
        }
    }
}
