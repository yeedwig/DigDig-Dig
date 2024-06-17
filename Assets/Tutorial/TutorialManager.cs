using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int currentStage = 0;
    private int previousStage = -1;

    [SerializeField] GameObject[] AndrewTheShrew;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject MoleHole;
    // Start is called before the first frame update
    void Start()
    {
        MoleHole.SetActive(false);
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
                    AndrewSetActive(0);
                    //�� ������ �ѹ��� ������ ��
                    //���� ��� UI ����̳� ���
                    break;

                case 1:
                    AndrewSetActive(1);
                    break;

                case 2:
                    AndrewSetActive(2);

                    break;

                case 3:
                    AndrewSetActive(3);
                    break;

                case 4:
                    AndrewSetActive(4);
                    break;

                case 5:
                    AndrewSetActive(5);
                    break;

                case 6:
                    AndrewSetActive(6);
                    break;

                case 7:
                    AndrewSetActive(7);
                    break;

                case 8:
                    AndrewSetActive(8);
                    MoleHole.SetActive(true);
                    break;
            }
        }
        //���� ���������� ���� ����
        switch(currentStage)
        {
            case 0:
                if (AndrewTheShrew[0].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 1:
                if (AndrewTheShrew[1].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 2:
                if (AndrewTheShrew[2].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 3:
                if (AndrewTheShrew[3].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 4:
                if (AndrewTheShrew[4].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 5:
                if (AndrewTheShrew[5].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 6:
                if (AndrewTheShrew[6].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 7:
                if (AndrewTheShrew[7].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;

            case 8:
                if (AndrewTheShrew[8].GetComponent<Andrew>().goToNextchapter == true)
                {
                    currentStage++;
                }
                break;
        }
    }

    public void Skip()
    {
        if(currentStage < 9)
        {
            currentStage++;
        }
        else { return; }
    }

    private void AndrewSetActive(int index)
    {
        for(int i = 0; i < AndrewTheShrew.Length; i++)
        {
            if (i == index)
                AndrewTheShrew[i].SetActive(true);
            else
                AndrewTheShrew[i].SetActive(false);
        }
    }
}
