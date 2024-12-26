using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DailyType
{
    Done,
    Collect,
    NotYet
}

public class DailyController : MonoBehaviour
{

    [HideInInspector] public float dailyMoney;
    [SerializeField] string dailyMessage;
    [SerializeField] DailyType dailyType;

    [SerializeField] Text moneyText;
    [SerializeField] Text dailyText;
    [SerializeField] GameObject done;
    [SerializeField] GameObject collect;
    [SerializeField] GameObject notYet;
    [HideInInspector] public bool waiting;
    void Start()
    {
        // ramden çekip karar ver
        if(waiting)
            setButton(DailyType.NotYet);
        else 
            setButton(DailyType.Done);

        moneyText.text = dailyMoney.ToString();
        dailyText.text = dailyMessage.ToString();
    }

    void setButton(DailyType daily)
    {
        switch(daily)
        {
            case DailyType.Collect:
                collect.SetActive(true);
                done.SetActive(false);
                notYet.SetActive(false);
                break;
            case DailyType.Done:
                collect.SetActive(false);
                done.SetActive(true);
                notYet.SetActive(false);
                break;
            case DailyType.NotYet:
                collect.SetActive(false);
                done.SetActive(false);
                notYet.SetActive(true);
                break;
        }
    }
}
