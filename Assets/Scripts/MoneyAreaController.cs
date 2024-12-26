using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MoneyAreaController : MonoBehaviour
{
    public float moneyCap;
    public float moneyAmount;
    [HideInInspector]public List<GameObject> moneys;
    [SerializeField] Transform moneyPos;
    [Range(0,5)] [SerializeField] float ObjectSpacingHeight;
    [SerializeField] GameObject objectPrefab;
    [SerializeField] MarketOpener marketOpener;
    [SerializeField] GameObject moneyMax;
    [SerializeField] TextMeshProUGUI moneyAmountText;

    private void Awake()
    {
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            addMoney();
    }

    private void LoadData()
    {
        moneyTextWriter();
        var moneysCount = PlayerPrefs.GetInt(marketOpener.MarketId + "moneysCount", 0);
        for (int i = 0; i < moneysCount; i++) addMoney();

    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(marketOpener.MarketId + "moneysCount", moneys.Count);
    }

    public void addMoney()
    {
        if (moneys.Count >= moneyCap)
            return;

        var x = Instantiate(objectPrefab, moneyPos);
        x.transform.parent = moneyPos;
        x.transform.position += transform.up * moneys.Count * ObjectSpacingHeight;
        moneys.Add(x);
        moneyTextWriter();
        SaveData();
    }
 
    void moneyTextWriter()
    {
        if (moneys.Count == moneyCap)
        {
            moneyMax.SetActive(true);
            moneyAmountText.gameObject.SetActive(false);
        }
        else
        {
            moneyAmountText.text = (moneyAmount * moneys.Count)  + "/" + (moneyCap * moneyAmount);
            moneyAmountText.gameObject.SetActive(true);
            moneyMax.SetActive(false);
        }
    }
    public void removeMoney()
    {
        var x = moneys.Last();
        moneys.Remove(x);
        Destroy(x);
        x = null;
        moneyTextWriter();
        SaveData();
    }
}
