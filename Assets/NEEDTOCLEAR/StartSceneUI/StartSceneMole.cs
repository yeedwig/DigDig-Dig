using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneMole : MonoBehaviour
{
    [SerializeField] Transform[] moleSpawnPositions;
    [SerializeField] GameObject startMole;
    public GameObject moleLight;
    private Animator anim;

    public float spawnTime;
    [SerializeField] private float spawnTimer;
    private bool isInCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        anim = startMole.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnTime && !isInCoroutine)
        {
            moleLight.SetActive(true);
            startMole.SetActive(true);
            anim.SetTrigger("DigOut");
            startMole.transform.position = moleSpawnPositions[Random.Range(0, 13)].transform.position;

            isInCoroutine = true;

            StartCoroutine(MoleBehave());
            //anim.SetTrigger("DiggIng");
        }
    }

    IEnumerator MoleBehave()
    {

        anim.SetTrigger("Idle");
        float waitSeconds = Random.Range(5.0f, 15.0f);

        yield return new WaitForSeconds(waitSeconds);

        anim.SetTrigger("Digging");
        moleLight.SetActive(false);
        
        //startMole.SetActive(false);
        isInCoroutine = false;
        spawnTimer = 0;
    }

}
