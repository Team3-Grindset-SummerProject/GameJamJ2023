using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageTestScript : MonoBehaviour
{
    [SerializeField] private int damage = 0;
    [SerializeField] private int slowSpeed = 0;
    private GameObject bigBadGuy = null;
    [SerializeField] private GameObject myself = null;
    private float xDistance = 0;
    private float yDistance = 0;
    [SerializeField] private float effectDistancex = 0.5f;
    [SerializeField] private float effectDistancey = 0.5f;
    [SerializeField] private bool isHurt = false;
    [SerializeField] private bool isGlue = false;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private GameObject blackScrene = null;
    [SerializeField] private AudioSource monsterScreem = null;
    private AudioManager a;
    
    public AudioClip m;
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

        if(xDistance <= effectDistancex && yDistance <= effectDistancey)
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
            if (isPlayer)
            {
                StartCoroutine(LoseGame());
                return;
            }
            if(!isGlue && !isHurt && !isPlayer)
            {
                ReverseBadGuy();
            }
        }

    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void AttackBadGuy()
    {
        bool shouldSlow = true;
        if (isGlue)
        {
            shouldSlow = false;
        }
        Debug.Log("High Yay");
        bigBadGuy.GetComponent<BigBadBehavior>().HurtEnemy(damage, shouldSlow);
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

    private IEnumerator LoseGame()
    {
        blackScrene.SetActive(true);
        monsterScreem.Play();
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
