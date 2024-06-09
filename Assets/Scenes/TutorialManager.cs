using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int currentStage = 0;
    private int previousStage = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //스테이지 변경 및 출력
        if (currentStage != previousStage)
        {
            previousStage = currentStage;
            switch(currentStage)
            {
                case 0:
                    //각 레벨당 한번만 실행할 것
                    //예를 들어 UI 출력이나 등등
                    break;
            }
        }
        //다음 스테이지로 진급 조건
        switch(currentStage)
        {
            case 0:
                if (false)
                {
                    currentStage++;
                }
                break;
        }
    }
}
