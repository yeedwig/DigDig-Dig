using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour 
{
    string name;
    public int ToolId; // 고유 아이디
    public int itemType; //0 = Shovel, 1 = Drill, 2 = TNT, 3 = Radar
    public float damage;

}

// 아이템 종류(삽인가 드릴간 티엔티인가
// 개별 ID (아이템끼리 -> 같은 타입이라도 데미지랑 다르니까)