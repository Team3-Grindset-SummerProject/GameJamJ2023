using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    
    [Header("Values")]
    [SerializeField] private float range;
    [SerializeField] private Vector3 trapOffset;
    
    [Header("Debug")]
    public bool trapPlaced;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(trapOffset + transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

    public float GetRange()
    {
        return range;
    }

    public Vector3 GetTrapPosition()
    {
        return transform.position + trapOffset;
    }
}
