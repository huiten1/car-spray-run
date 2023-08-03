using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionListener : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        other.GetComponentInParent<IInteractable>()?.Interact(gameObject);
    }
}
