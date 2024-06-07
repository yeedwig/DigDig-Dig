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

    [SerializeField] InventoryManager inventoryManager;

    private int curToolEfficiency;

    private string[] itemTestName = new string[] {"Dirt","Stone","Diamond"}; 
    private int[] itemTestNum = new int[] {0,0,0}; // 나중에 자원 id 고려
    private int[] itemTestNumCombo = new int[]{0,0,0}; // 나중에 자원 id 고려

    private float itemGetTextTimer=0;
    [SerializeField] float itemGetTextTimerMax;

    [SerializeField] Text combo;
    public static ItemDropManager instance = null;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        toolManager = toolManagerObj.GetComponent<ToolManager>();
        itemGetTextTimer = itemGetTextTimerMax;
        combo.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        itemGetTextTimer += Time.deltaTime;
        //ShowItemTotal(itemTestNum);
        ShowItemCombo(itemTestNumCombo);
    }
    public void GetItem(GroundSO groundSO) //중요 <- ground.cs에서 옴
    {
        itemGetTextTimer = 0;
        curToolEfficiency = toolManager.curToolEfficiency;
        StartCoroutine(AddItem(groundSO.resources));
    }

    IEnumerator AddItem(Item[] resources) //중요
    {
        int randomNum;
        string totalresult = "";
        for(int i =0;i<curToolEfficiency;i++)
        {
            randomNum = Random.Range(0, resources.Length);
            //itemTestNum[resources[randomNum].resourceId] += 1; //이거 인벤에 추가
            inventoryManager.AddItem(resources[randomNum]);
            //StartCoroutine(ShowAddedItem(resources[randomNum]));
            itemTestNumCombo[resources[randomNum].resourceId] += 1;
            yield return null;
        }
    }
    /*
    private void ShowItemTotal(int[] arr)
    {
        string result = "";
        for (int i = 0; i < arr.Length; i++)
        {
            result = result + itemTestName[i] + ": " + arr[i].ToString() + "\n";
        }
        total.text = result;
    }*/

    
    private void ShowItemCombo(int[] arr)
    {
        string result = "";

        if (itemGetTextTimer < itemGetTextTimerMax)
        {
            //combo.color = new Color(combo.color.r, combo.color.g, combo.color.b, 1);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                {
                    result = result + itemTestName[i] + " + " + arr[i].ToString() + "\n";
                }
            }
        }
        else
        {
            itemTestNumCombo = Enumerable.Repeat<int>(0,itemTestNumCombo.Length).ToArray<int>();
        }

        combo.text = result;
        Animator comboAnim = combo.GetComponent<Animator>();
        comboAnim.SetTrigger("PopUp");
        //StartCoroutine(FadeTextToZero(combo));

    }

    public IEnumerator FadeTextToZero(Text text)  // 알파값 1에서 0으로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
    }

    // 한개씩 보여주는것
    IEnumerator ShowAddedItem(Item item)
    {
        combo.text = item.name.ToString() + " + 1";
        yield return new WaitForSeconds(0.5f);
        combo.text = "";
    }
}
