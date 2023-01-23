using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySummoned : MonoBehaviour
{
    [SerializeField] private GameObject enemy = null;
    [SerializeField] private GameObject enemySprite = null;
    [SerializeField] private GameObject player = null;

    private float xPos = 0.0f;
    private float yPos = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xPos = Mathf.Abs(player.transform.position.x - this.transform.position.x);
        yPos = Mathf.Abs(player.transform.position.y - this.transform.position.y);
        if(xPos <= 5 && yPos <= 5)
        {
            SummonHim();
        }
    }

    private void SummonHim()
    {
        enemySprite.SetActive(true);
        enemy.GetComponent<BigBadBehavior>().enabled = !enemy.GetComponent<BigBadBehavior>().enabled;
        enemy.GetComponent<NavMeshAgent>().enabled = !enemy.GetComponent<NavMeshAgent>().enabled;
        Destroy(this);
    }

}
