using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    private BGMManager bgmManager;
    [SerializeField] private int changeIndex;
    [SerializeField] private float changeVolume;
    private void Start()
    {
        bgmManager = FindFirstObjectByType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bgmManager.changeBGM(changeIndex, changeVolume);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bgmManager.changeBGM(0, 0.5f);
        }
    }
}
