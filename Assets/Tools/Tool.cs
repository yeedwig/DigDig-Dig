using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour 
{
    string name;
    public int ToolId; // ���� ���̵�
    public int itemType; //0 = Shovel, 1 = Drill, 2 = TNT, 3 = Radar
    public float damage;

}

// ������ ����(���ΰ� �帱�� Ƽ��Ƽ�ΰ�
// ���� ID (�����۳��� -> ���� Ÿ���̶� �������� �ٸ��ϱ�)