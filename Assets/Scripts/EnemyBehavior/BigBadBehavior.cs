using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BigBadBehavior : MonoBehaviour
{
    private int health = 5;
    [SerializeField] private float enemySpeed = 5, enemyBaseSpeed = 5;
    private GameObject player = null;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private GameObject reversePoint = null;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySpeed = enemyBaseSpeed;

        transform.position = reversePoint.transform.position;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        agent.speed = enemySpeed;
    }

    public void SlowEnemy (int slowStrength, float slowTime)
    {
        enemySpeed -= slowStrength;
        StartCoroutine(EnemySlowLegnth(slowTime));
    }

    private IEnumerator EnemySlowLegnth(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        enemySpeed = enemyBaseSpeed;
    }

    public void HurtEnemy(int damage, bool shouldSlow)
    {
        health -= damage;
        if(health <= 0)
        {
            SlowEnemy(10, 2.0f);
            health = 5;
            return;
        }
        if (shouldSlow)
        {
            SlowEnemy(10, 0.15f);
        }
    }

    public IEnumerator EnemyReversal()
    {
        player = reversePoint;
        yield return new WaitForSeconds(2.0f);
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
