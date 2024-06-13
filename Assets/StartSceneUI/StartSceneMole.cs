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
            
            isInCoroutine = true;
            startMole.transform.position = moleSpawnPositions[Random.Range(0,13)].position;
            startMole.SetActive(true);
            moleLight.SetActive(true);
            anim.SetTrigger("DigOut");
            Debug.Log("Out");
            StartCoroutine(MoleBehave());
        }
    }

    IEnumerator MoleBehave()
    {
        Debug.Log("Idle");
        anim.SetTrigger("Idle");
        float waitSeconds = Random.Range(10.0f, 20.0f);
        yield return new WaitForSeconds(waitSeconds);
        anim.SetTrigger("DiggIng");
        moleLight.SetActive(false);
        Debug.Log("DigIn");
        //startMole.SetActive(false);
        isInCoroutine = false;
        spawnTimer = 0;
    }

}
