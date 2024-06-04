using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int curTutorialLevel;
    [SerializeField] private GameObject[] gameMessage;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public ToolManager toolManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(curTutorialLevel == 0)
        {
            gameMessage[0].SetActive(true);
        }
        if(curTutorialLevel == 1)
        {
            gameMessage[0].SetActive(false);
            gameMessage[1].SetActive(true);
        }
    }


    private void TutorialCheck()
    {
        if(curTutorialLevel == 0)
        {
            
        }
    }


}
