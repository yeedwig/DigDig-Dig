using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public bool AntNestFound;
    public bool ArmyTrenchFound;
    public bool MadScientistLabFound;
    public bool CrusadeFound;
    public bool CatacombFound;
    public bool AtlantisFound;
    public bool UndergroundTribeFound;
    public bool LostWorldFound;
    public bool EldoradoFound;
    public bool TreasureFound;

    [SerializeField] private GameObject[] shopToolSlots;

    private void Start()
    {

    }
    private void Update()
    {
        CheckRuinFound();
    }


    private void CheckRuinFound()
    {
        if(AntNestFound)
        {
            shopToolSlots[0].SetActive(true);
        }
        if (ArmyTrenchFound)
        {
            shopToolSlots[1].SetActive(true);
        }
        if (MadScientistLabFound)
        {
            shopToolSlots[2].SetActive(true);
        }
        if (CrusadeFound)
        {
            shopToolSlots[3].SetActive(true);
        }
        if (CatacombFound)
        {
            shopToolSlots[4].SetActive(true);
        }
        if (AtlantisFound)
        {
            shopToolSlots[5].SetActive(true);
        }
        if (UndergroundTribeFound)
        {
            shopToolSlots[6].SetActive(true);
        }
        if (LostWorldFound)
        {
            shopToolSlots[7].SetActive(true);
        }
        if (EldoradoFound)
        {
            shopToolSlots[8].SetActive(true);
        }
        if(TreasureFound)
        {
            shopToolSlots[9].SetActive(true);
        }

    }
}
