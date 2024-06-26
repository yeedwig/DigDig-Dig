using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChangeBtns : MonoBehaviour
{
    public GameObject[] ShopUILists;
    public AudioClip[] ButtonSound;

    public void Start()
    {
        ShopUILists[0].SetActive(true);
        ShopUILists[1].SetActive(false);
        ShopUILists[2].SetActive(false);
        ShopUILists[3].SetActive(false);
    }
    public void SetActiveShovelShop()
    {
        SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 1.5f);
        ShopUILists[0].SetActive(true);
        ShopUILists[1].SetActive(false);
        ShopUILists[2].SetActive(false);
        ShopUILists[3].SetActive(false);
    }

    public void SetActiveDrillShop()
    {
        SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 1.5f);
        ShopUILists[0].SetActive(false);
        ShopUILists[1].SetActive(true);
        ShopUILists[2].SetActive(false);
        ShopUILists[3].SetActive(false);
    }

    public void SetActiveToolStructShop()
    {
        SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 1.5f);
        ShopUILists[0].SetActive(false);
        ShopUILists[1].SetActive(false);
        ShopUILists[2].SetActive(true);
        ShopUILists[3].SetActive(false);
    }

    public void SetActiveOtherShop()
    {
        SoundFXManager.instance.PlaySoundFXClip(ButtonSound, transform, 1.5f);
        ShopUILists[0].SetActive(false);
        ShopUILists[1].SetActive(false);
        ShopUILists[2].SetActive(false);
        ShopUILists[3].SetActive(true);
    }
}
