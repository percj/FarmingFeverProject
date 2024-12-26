using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StationOpener : MonoBehaviour
{
    [Header("=== Identifier ===")]
    public string ID;
    public string StationName;


    [SerializeField] Transform marketOpenTeleportPos;
    [SerializeField] GameObject player;
    [SerializeField] GameObject openStation;
    [SerializeField] GameObject unlockStation;
    [SerializeField] CustomerSpawner spawner;

    public float TotalMoney;
    bool canGiveMoney;
    [HideInInspector] public float investedPrice;
    [SerializeField] Image MoneyFiller;
    [SerializeField] TextMeshProUGUI MoneyText;
    [HideInInspector]public bool isOpen;
    void Start()
    {
        openStation.SetActive(false);
        LoadData();
        refreshMoney();
        Payment(0);
        LoadOpenedTypesData();
    }

    private void LoadOpenedTypesData()
    {
        var a = PlayerPrefs.GetInt(StationName + "oponedTypes");
        if (a == 1 && !spawner.openedLandType.Contains(CarryObjectType.EggPlant))
            spawner.openedLandType.Add(CarryObjectType.EggPlant);
        if (a == 3 && !spawner.openedLandType.Contains(CarryObjectType.Carrot))
            spawner.openedLandType.Add(CarryObjectType.Carrot);
        if (a == 2 && !spawner.openedLandType.Contains(CarryObjectType.Corn))
            spawner.openedLandType.Add(CarryObjectType.Corn);
    }
    private void LoadData()
    {
        investedPrice = PlayerPrefs.GetFloat(ID + StationName + "investedPrice", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(ID + StationName + "investedPrice", investedPrice);
    }
    void refreshMoney()
    {

        MoneyFiller.fillAmount = investedPrice / TotalMoney;
        MoneyText.text = (TotalMoney - investedPrice).ToString();
        SaveData();
    }

    public float Payment(float givenPrice)
    {
        if (TotalMoney - investedPrice > 0)
        {
            investedPrice += givenPrice;
            refreshMoney();
            if (TotalMoney - investedPrice <= 0)
            {
                if (StationName == "Market")
                {
                    player.GetComponent<CharacterController>().enabled = false;
                    player.transform.position = marketOpenTeleportPos.position;
                    player.GetComponent<CharacterController>().enabled = true;

                }
                GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.OpenMarket);

                openStation.SetActive(true);
                unlockStation.SetActive(false);
                isOpen = true;

                if (openStation.tag == "EggPlantLand" && !spawner.openedLandType.Contains(CarryObjectType.EggPlant))
                {
                    spawner.openedLandType.Add(CarryObjectType.EggPlant);
                    PlayerPrefs.SetInt(StationName + "oponedTypes", 1);
                }  
                if (openStation.tag == "CarrotLand" && !spawner.openedLandType.Contains(CarryObjectType.Carrot))
                {
                    spawner.openedLandType.Add(CarryObjectType.Carrot);
                    PlayerPrefs.SetInt(StationName + "oponedTypes", 3);
                }
                if (openStation.tag == "CornLand" && !spawner.openedLandType.Contains(CarryObjectType.Corn))
                {
                    spawner.openedLandType.Add(CarryObjectType.Corn);
                    PlayerPrefs.SetInt(StationName + "oponedTypes", 2);
                }
            }

            return -givenPrice;
        }
        else
        {
            openStation.SetActive(true);
            unlockStation.SetActive(false);
            isOpen = true;
            return -(TotalMoney - investedPrice);
        }
    }
}
