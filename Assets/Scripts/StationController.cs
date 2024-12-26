using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationController : MonoBehaviour
{

    [Header("=== Identifier ===")]
    public string ID;
    public string StationName;


    [Header("=== Needed ===")]
    /*
    (helpercontroller)
    (unlockedmanager activate!)
    [SerializeField] List<Transform> stationObjectPos;
    [SerializeField] List<Transform> objectPos;
    */
    [HideInInspector] public CustomerController currCustomer;
    public Transform intract;
    [HideInInspector] public List<GameObject> stationObjects;
    public GameObject objectPrefab;
    [Range(0,5)][SerializeField] float ObjectSpacingHeight;
    [SerializeField] Transform objectPos;
    public UnlockedManager unlockedManager;
    public MoneyAreaController moneyAreaController;
    public Transform stationObjectPos;

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {                                           // 1CustomerStationisUnlocked
        var stationObjectsCount = PlayerPrefs.GetInt(ID + StationName + "stationObjectsCount", 0);
        for (int i = 0; i < stationObjectsCount; i++) addObject();

    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(ID + StationName + "stationObjectsCount", stationObjects.Count);
    }


    public void addObject()
    {
        if(unlockedManager.isUnlocked)
        {
            var x = Instantiate(objectPrefab, objectPos);
            x.transform.parent = objectPos;
            x.transform.position += transform.up * stationObjects.Count * ObjectSpacingHeight;
            stationObjects.Add(x);
        }
        SaveData();
    }

    public void removeObject()
    {
        if (unlockedManager.isUnlocked)
        {
            var x = stationObjects.Last();
            stationObjects.Remove(x);
            Destroy(x);
            x = null;
        }
        SaveData();
    }
}
