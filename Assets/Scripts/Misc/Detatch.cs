using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detatch : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }
}
