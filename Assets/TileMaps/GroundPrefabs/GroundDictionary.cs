using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDictionary : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> groundDictionary;
    void Start()
    {
       groundDictionary=new Dictionary<Vector3Int, GameObject>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int x, y;
            x = Random.Range(-12, 3);
            y = Random.Range(-8, -3);
            
            for(int i = x - 1; i <= x + 1; i++)
            {
                for(int j = y - 1;j<= y + 1; j++)
                {
                    Destroy(groundDictionary[new Vector3Int(i,j,0)]);
                }
            }
            
        }
    }
}
