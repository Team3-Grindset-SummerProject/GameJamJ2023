using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private BigBadBehavior enemy;
    [SerializeField] private GameObject activationObject;

    [SerializeField] private bool isActive, used;

    [SerializeField] private float range;
    [SerializeField] private int damage, slow;
    [SerializeField] private float slowTime;
    [SerializeField] private bool reverse;

    private void Start()
    {
        enemy = GameObject.FindWithTag("BadGuy").GetComponent<BigBadBehavior>();
    }

    private void Update()
    {
        if (!isActive) return;
        
        if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
        {
            enemy.SlowEnemy(slow, slowTime);
            enemy.HurtEnemy(damage, false);

            if (reverse)
                StartCoroutine(enemy.EnemyReversal());

            isActive = false;

            used = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (used)
            return;
        
        if (other.CompareTag("Player"))
        {
            isActive = true;
            activationObject.SetActive(true);
        }
    }
}
