using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BigBadBehavior : MonoBehaviour
{
    private int health = 5;
    private float enemySpeed = 10;
    private GameObject player = null;
    [SerializeField] private NavMeshAgent agent = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        agent.speed = enemySpeed;
    }

    public void SlowEnemy (int newSpeed, float slowTime)
    {
        enemySpeed = newSpeed;
        StartCoroutine(enemySlowLegnth(slowTime));
    }

    private IEnumerator enemySlowLegnth(float slowTime)
    {
        yield return new WaitForSeconds(slowTime);
        enemySpeed = 10;
    }

    public void hurtEnemy(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            SlowEnemy(0, 4.0f);
        }
    }
}
