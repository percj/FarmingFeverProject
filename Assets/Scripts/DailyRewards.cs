using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewards : MonoBehaviour
{
    [SerializeField] List<GameObject> rewardsList;
    [SerializeField] GameObject rewardPrefab;
    [SerializeField] int startMoney;
    [SerializeField] int moneyMultiply;
    DateTime startDate;
    [HideInInspector] public DateTime lastCollectDate;
    void OnEnable()
    {
        foreach (GameObject reward in rewardsList)
        {
            Destroy(reward);
        }
        rewardsList = new List<GameObject>();
        InitialData();
        var stract = (lastCollectDate - startDate).Days;

        if(lastCollectDate == new DateTime())
        {
            var x = Instantiate(rewardPrefab);
            x.transform.parent = transform;
            var dailyReward = x.GetComponent<DailyRewardController>();
            dailyReward.moneyAmount = (startMoney * (moneyMultiply * 1));
            dailyReward.dayText.text += " " + 1;
            dailyReward.rewards = this;
            rewardsList.Add(x);
        }
        else
        {
            for (var i = 1; i <= stract+1; i++)
            {
                var x = Instantiate(rewardPrefab);
                x.transform.parent = transform;
                var dailyReward = x.GetComponent<DailyRewardController>();
                dailyReward.isDone = true;
                dailyReward.moneyAmount = (startMoney * (moneyMultiply * i));
                dailyReward.dayText.text += " " + i;
                dailyReward.rewards = this;
                rewardsList.Add(x);
            }
           
                stract += 1;
                var x2 = Instantiate(rewardPrefab);
                x2.transform.parent = transform;
                var  dailyReward2 = x2.GetComponent<DailyRewardController>();
                if (lastCollectDate.Day == DateTime.Today.Day) dailyReward2.isComeTomorrow = true;
                dailyReward2.moneyAmount = (startMoney * (moneyMultiply * ((int)stract+1)));
                dailyReward2.dayText.text += " " + (int)(stract + 1);
                dailyReward2.rewards = this;
                rewardsList.Add(x2);
          
        }


    }

    internal void addTomorrow()
    {
        var stract = (lastCollectDate - startDate).Days +2;
        var x = Instantiate(rewardPrefab);
        x.transform.parent = transform;
        var dailyReward = x.GetComponent<DailyRewardController>();
        dailyReward.isComeTomorrow = true;
        dailyReward.moneyAmount = (startMoney * (moneyMultiply * (int)stract));
        dailyReward.dayText.text += " " + (int)(stract);
        dailyReward.rewards = this;
        rewardsList.Add(x);
        SaveData();

    }
    private void SaveData()
    {
        PlayerPrefs.SetString("StartDate", startDate.ToString());
        PlayerPrefs.SetString("LastCollectDate", lastCollectDate.ToString());
    }

    private void InitialData()
    {
        
       var start = PlayerPrefs.GetString("StartDate");
        if (start != "")
            startDate = DateTime.Parse(start);
        var last = PlayerPrefs.GetString("LastCollectDate");
        if (last != "")
            lastCollectDate = DateTime.Parse(last);


        if (lastCollectDate != new DateTime())
        {
            if (lastCollectDate < DateTime.Today.AddDays(-1))
            {
                startDate = DateTime.Today;
                lastCollectDate = new DateTime();
            }
        }
        else startDate = DateTime.Today;
    }
}
