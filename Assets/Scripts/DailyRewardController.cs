using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardController : MonoBehaviour
{
    public Text dayText;
    [SerializeField] Text moneyText;
    [HideInInspector] public float moneyAmount;
    [HideInInspector] public bool isDone;
    [SerializeField] GameObject Collect;
    [SerializeField] GameObject Done;
    [SerializeField] GameObject ComeTomorrow;
    [HideInInspector] public bool isComeTomorrow;
    [HideInInspector] public DailyRewards rewards;
    void Start()
    {
        transform.localScale = Vector3.one;
        if (isDone)
        {
            Done.SetActive(true);
        }
        else if(isComeTomorrow )
        {
            ComeTomorrow.SetActive(true);
        }
        else 
        {
            Collect.SetActive(true); 
        }

        //moneyText.text = collectDate.ToShortDateString();
        moneyText.text = LunesHelper.CrunchNumbers(moneyAmount);
    }

    public void collect()
    {
        Collect.SetActive(false);
        Done.SetActive(true);
        rewards.lastCollectDate = DateTime.Now;
        rewards.addTomorrow();
        GameSingleton.Instance.SetMoney(moneyAmount);
    }
}
