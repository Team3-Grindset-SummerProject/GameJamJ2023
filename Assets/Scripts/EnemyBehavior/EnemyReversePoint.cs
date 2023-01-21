using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReversePoint : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private GameObject badGuy = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(badGuy.transform.position);
    }
}
