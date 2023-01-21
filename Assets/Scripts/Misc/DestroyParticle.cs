using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] private float lifetime;
    
    void Start()
    {
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
