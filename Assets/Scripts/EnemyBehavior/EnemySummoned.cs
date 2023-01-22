using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySummoned : MonoBehaviour
{
    [SerializeField] private GameObject enemy = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.gameObject.CompareTag("Player"))
        {
            enemy.GetComponent<BigBadBehavior>().enabled = !enemy.GetComponent<BigBadBehavior>().enabled;
            enemy.GetComponent<NavMeshAgent>().enabled = !enemy.GetComponent<NavMeshAgent>().enabled;

        }
    }
}
