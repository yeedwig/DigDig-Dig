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
        //�������� ���� �� ���
        if (currentStage != previousStage)
        {
            previousStage = currentStage;
            switch(currentStage)
            {
                case 0:
                    //�� ������ �ѹ��� ������ ��
                    //���� ��� UI ����̳� ���
                    break;
            }
        }
        //���� ���������� ���� ����
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
