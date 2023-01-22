using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTestScript : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] private int slowSpeed = 0;
    private GameObject bigBadGuy = null;
    [SerializeField] private GameObject myself = null;
    [SerializeField] private float xDistance = 0.5f;
    [SerializeField] private float yDistance = 0.5f;
    [SerializeField] private bool isHurt = false;
    [SerializeField] private bool isGlue = false;
    //private float zDistance = 0;
    // Start is called before the first frame update
    void Start()
    {
        bigBadGuy = GameObject.FindGameObjectWithTag("BadGuy");
    }

    // Update is called once per frame
    void Update()
    {
        xDistance = Mathf.Abs(transform.position.x - bigBadGuy.transform.position.x);
        yDistance = Mathf.Abs(transform.position.y - bigBadGuy.transform.position.y);
        //zDistance = Mathf.Abs(transform.position.z - bigBadGuy.transform.position.z);

        if(xDistance <= 0.5 && yDistance <= 0.5)
        {

            if (isGlue)
            {
                SlowBadGuy();
            }
            if (isHurt)
            {
                AttackBadGuy();
                return;
            }
            if(!isGlue && !isHurt)
            {
                ReverseBadGuy();
            }
        }

    }

    private void AttackBadGuy()
    {
        bool shouldSlow = true;
        if (isGlue)
        {
            shouldSlow = false;
        }
        Debug.Log("High Yay");
        bigBadGuy.GetComponent<BigBadBehavior>().hurtEnemy(damage, shouldSlow);
        Destroy(myself);
    }

    private void ReverseBadGuy()
    {
        bigBadGuy.GetComponent<BigBadBehavior>().StartCoroutine("EnemyReversal");
        Destroy(myself);
    }

    private void SlowBadGuy()
    {
        bigBadGuy.GetComponent<BigBadBehavior>().SlowEnemy(slowSpeed, 3.0f);
        Destroy(myself);
    }

}
