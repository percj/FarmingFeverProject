using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCalculator : MonoBehaviour
{
    int counter;
    [SerializeField] float dailyMoney;
    List<GameObject> _dailys;
    [SerializeField] GameObject daily;

    void Start() => Initializedata();

    private void Initializedata()
    {
        _dailys = new List<GameObject>();
        counter = PlayerPrefs.GetInt("DailyCounter", 4);

       for(var i = 0; i< counter; i++)
        {
            if(i==counter-1)
            {
                addDaily(true);
            }
            else
            {
                addDaily(false);
            }

        }
    }

    void addDaily(bool isWaiting)
    {
        var x = Instantiate(daily,transform);
        x.transform.parent = transform;
        var controller = x.GetComponent<DailyController>();
        controller.waiting = isWaiting;
        controller.dailyMoney = counter * dailyMoney;
        _dailys.Add(x);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("DailyCounter", counter);
    }

    // Update is called once per frame
    void Update()
    {
        SaveData();
    }


}
