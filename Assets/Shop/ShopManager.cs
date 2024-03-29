using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public bool AntNestFound;
    public bool ArmyTrenchFound;
    public bool PiratesMet;
    public bool CrusadeFound;
    public bool CatacombFound;
    public bool AtlantisFound;
    public bool UndergroundTribeFound;
    public bool LostWorldFound;
    public bool EldoradoFound;
    public bool TreasureFound;

    [SerializeField] private GameObject[] shopShovelSlots;
    [SerializeField] private GameObject[] shopDrillSlots;

    private void Start()
    {
        AntNestFound = false;
        ArmyTrenchFound = false;
        PiratesMet = false;
        CrusadeFound = false;
        CatacombFound = false;
        AtlantisFound = false;
        UndergroundTribeFound = false;
        LostWorldFound = false;
        EldoradoFound = false;
        TreasureFound = false;

        for(int i=0; i<shopShovelSlots.Length; i++)
        {
            shopDrillSlots[i].SetActive(false);
            shopDrillSlots[i].SetActive(false);
        }
    }
    private void Update()
    {
        CheckRuinFound();
    }


    private void CheckRuinFound()
    {
        if(AntNestFound)
        {
            shopShovelSlots[0].SetActive(true);
            shopDrillSlots[0].SetActive(true);  
        }
        if (ArmyTrenchFound)
        {
            shopShovelSlots[1].SetActive(true);
            shopDrillSlots[1].SetActive(true);
        }
        if (PiratesMet)
        {
            shopShovelSlots[2].SetActive(true);
            shopDrillSlots[2].SetActive(true);
        }
        if (CrusadeFound)
        {
            shopShovelSlots[3].SetActive(true);
            shopDrillSlots[3].SetActive(true);
        }
        if (CatacombFound)
        {
            shopShovelSlots[4].SetActive(true);
            shopDrillSlots[4].SetActive (true);
        }
        if (AtlantisFound)
        {
            shopShovelSlots[5].SetActive(true);
            shopDrillSlots[5].SetActive (true);
        }
        if (UndergroundTribeFound)
        {
            shopShovelSlots[6].SetActive(true);
            shopDrillSlots[6].SetActive(true);
        }
        if (LostWorldFound)
        {
            shopShovelSlots[7].SetActive(true);
            shopDrillSlots[7].SetActive (true);
        }
        if (EldoradoFound)
        {
            shopShovelSlots[8].SetActive(true);
            shopDrillSlots[8].SetActive(true);
        }
        if(TreasureFound)
        {
            //shopShovelSlots[9].SetActive(true);
            //shopDrillSlots[9].SetActive (true);
        }
    }

}
