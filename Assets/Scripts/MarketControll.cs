using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MarketControll : MonoBehaviour
{
    public CarryObjectType objectType;
    public List<GameObject> objects;
    public TextMeshProUGUI countText;
    public int currObjectAmount = 0;
    public Transform intractPos;
    public MoneyAreaController moneyAreaController;

    public StationOpener stationOpener;


    public List<GameObject> queue;
    public List<GameObject> currentCustomers;

    public GameObject observePos;

    public SellArea sellArea;

    [Header("Identifiers")]
    public int Id;
    public MarketOpener marketOpener;

    private void LoadData()
    {
        currObjectAmount = PlayerPrefs.GetInt(marketOpener.MarketId + "" + Id + "currentSellObjectCount", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(marketOpener.MarketId + "" + Id + "currentSellObjectCount", objects.Where(x => x.activeInHierarchy).ToList().Count);
    }

    void Start()
    {
        LoadData();
        refresh();
        countText.text = objects.Where(x=> x.activeInHierarchy == true).ToList().Count.ToString()+ "/"+ objects.Count.ToString();
    }

    void refresh()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (currObjectAmount > i)
                objects[i].SetActive(true);
            else
                objects[i].SetActive(false);
        }
        countText.text = objects.Where(x=> x.activeInHierarchy == true).ToList().Count.ToString()+ "/"+ objects.Count.ToString();
    } 

    public bool AddObject()
    {
        if(currObjectAmount < objects.Count)
        {
            currObjectAmount +=1;
            refresh();
            SaveData();
            return true;
        }
        return false;
    }
    public bool RemoveObject()
    {
        if (currObjectAmount > 0)
        {
            currObjectAmount -= 1;
            refresh();
            SaveData();
            return true;
        }
        return false;

    }
}
