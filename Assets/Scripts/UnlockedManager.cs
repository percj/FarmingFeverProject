using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedManager : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] StationController station;
    [SerializeField] List<GameObject> activatedGameObject;
    [SerializeField] List<GameObject> deactivatedGameObject;
    [SerializeField] Image MoneyFiller;
    [SerializeField] TextMeshProUGUI MoneyText;
    public float TotalMoney;
    public float investedPrice;
    public bool isUnlocked = false;


    void Awake()
    {
        LoadData();
        refreshMoney();
    }

    private void LoadData()
    {                                           // 1CustomerStationisUnlocked
       
        var investedPrice = PlayerPrefs.GetFloat(station.ID + station.StationName + "investedPrice", 0);
        this.investedPrice = investedPrice;
        if (TotalMoney - investedPrice <= 0) unlock(true);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(station.ID + station.StationName + "investedPrice", investedPrice);
    }


    void refreshMoney()
    {
        MoneyFiller.fillAmount = investedPrice / TotalMoney;
        MoneyText.text = (TotalMoney - investedPrice).ToString();
        SaveData();
    }

    void unlock(bool isLoad)
    {
        if (!isLoad) GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.OpenStation);
        isUnlocked = true;
        foreach (GameObject go in activatedGameObject)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in deactivatedGameObject)
        {
            go.SetActive(false);
        }
    }

    public float Payment(float givenPrice)
    {
        if(TotalMoney - investedPrice > 0)
        {
            investedPrice += givenPrice;
            refreshMoney();
            return -givenPrice;
        }
        else
        {
            unlock(false);
            return -(TotalMoney - investedPrice);
        }
    }
}
