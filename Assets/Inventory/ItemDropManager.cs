using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropManager : MonoBehaviour
{
    [SerializeField] GameObject toolManagerObj;
    private ToolManager toolManager;
    private int curToolEfficiency;

    private string[] itemTestName = new string[] {"Dirt","Stone","Diamond"}; 
    private int[] itemTestNum = new int[] {0,0,0}; // 나중에 자원 id 고려
    private int[] itemTestNumCombo = new int[] { 0, 0, 0 }; // 나중에 자원 id 고려

    private float itemGetTextTimer=0;
    [SerializeField] float itemGetTextTimerMax;

    [SerializeField] Text total;
    [SerializeField] Text combo;


    // Start is called before the first frame update
    void Start()
    {
        toolManager = toolManagerObj.GetComponent<ToolManager>();
        itemGetTextTimer = itemGetTextTimerMax;
        total.text = "";
        combo.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        itemGetTextTimer += Time.deltaTime;
        ShowItemTotal(itemTestNum);
        ShowItemCombo(itemTestNumCombo);
    }
    public void GetItem(GroundSO groundSO) //중요 <- ground.cs에서 옴
    {
        itemGetTextTimer = 0;
        curToolEfficiency = toolManager.curToolEfficiency;
        StartCoroutine(AddItem(groundSO.resources));
    }

    IEnumerator AddItem(Resource[] resources) //중요
    {
        int randomNum;
        for(int i =0;i<curToolEfficiency;i++)
        {
            randomNum = Random.Range(0, resources.Length);
            itemTestNum[resources[randomNum].resourceId] += 1; //이거 인벤에 추가
            itemTestNumCombo[resources[randomNum].resourceId] += 1;
            yield return null;
        }
    }

    private void ShowItemTotal(int[] arr)
    {
        string result = "";
        for (int i = 0; i < arr.Length; i++)
        {
            result = result + itemTestName[i] + ": " + arr[i].ToString() + "\n";
        }
        total.text = result;
    }

    private void ShowItemCombo(int[] arr)
    {
        string result = "";
        if (itemGetTextTimer < itemGetTextTimerMax)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                {
                    result = result + itemTestName[i] + ": " + arr[i].ToString() + "\n";
                }
            }
        }
        else
        {
            itemTestNumCombo = Enumerable.Repeat<int>(0,itemTestNumCombo.Length).ToArray<int>();
        }
        
        combo.text = result;
    }
}
