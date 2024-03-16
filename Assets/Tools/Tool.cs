using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour //0 = Shovel, 1 = Drill, 2 = TNT, 3 = Radar
{
    string name;
    public int ToolId;
    public string itemType;
    public float damage;

}

// 아이템 종류(삽인가 드릴간 티엔티인가
// 개별 ID (아이템끼리 -> 같은 타입이라도 데미지랑 다르니까)