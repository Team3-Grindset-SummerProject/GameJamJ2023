using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private BigBadBehavior enemy;
    [SerializeField] private GameObject activationObject;

    [SerializeField] private bool isActive;

    [SerializeField] private float range;
    [SerializeField] private int damage, slow;
    [SerializeField] private float slowTime;

    private void Start()
    {
        enemy = GameObject.FindWithTag("BadGuy").GetComponent<BigBadBehavior>();
    }

    private void Update()
    {
        if (isActive)
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                enemy.SlowEnemy(slow, slowTime);
                enemy.HurtEnemy(damage, false);

                isActive = false;
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            activationObject.SetActive(true);
        }
    }
}
