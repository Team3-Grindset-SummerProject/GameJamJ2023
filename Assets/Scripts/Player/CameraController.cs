using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private float cameraSpeed;

    private void Awake()
    {
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(followTransform.position.x, followTransform.position.y + 3, -12);
        
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraSpeed * Time.fixedDeltaTime);
    }
}
