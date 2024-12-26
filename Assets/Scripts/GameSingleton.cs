using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSingleton : MonoBehaviour
{
    public SoundManager Sounds;
    public UIManager UI;
    [HideInInspector]public float Money=0;
    [SerializeField] Text MoneyText;

    private static GameSingleton _instance;
    public static GameSingleton Instance { get { return _instance; } }

    private void Awake()
    {
        Money = PlayerPrefs.GetFloat("Money",Money);
        MoneyText.text = LunesHelper.CrunchNumbers(Money);
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            SetMoney(10000);
    }

    public void SetMoney(float setMoney)
    {
        if (setMoney > 0f) Sounds.audioSource.PlayOneShot(Sounds.CashCollect);
        Money += setMoney;
        MoneyText.text = Money.ToString();
        PlayerPrefs.SetFloat("Money", Money);
        MoneyText.text = LunesHelper.CrunchNumbers(Money);
    }
    public float getMoney()
    {
        return Money;
    }
}
