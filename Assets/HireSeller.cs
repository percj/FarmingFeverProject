using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HireSeller : MonoBehaviour
{
    [Header("=== Identifier ===")]
    public MarketOpener marketOpener;
    public string StationName;

    [SerializeField] List<GameObject> helpersPrefab;
    [SerializeField] Transform helperParent;
    [SerializeField] Transform waitingPos;
    [SerializeField] Transform lookAtPos;
    [SerializeField] List<MarketControll> markets;


    public float TotalMoney;
    bool canGiveMoney;
    [SerializeField] Canvas canvas;
    [HideInInspector] public float investedPrice;
    [SerializeField] Image MoneyFiller;
    [SerializeField] TextMeshProUGUI MoneyText;

    [SerializeField] GameObject sellArea;

    void Start()
    {
        LoadData();
        refreshMoney();
    }

    private void LoadData()
    {
        investedPrice = PlayerPrefs.GetFloat(marketOpener.MarketId + StationName + "investedPrice", 0);
        var sellerbought = PlayerPrefs.GetInt(marketOpener.MarketId + StationName + "seller", 0);
        if (sellerbought == 1) HireEmployee(true);
    }

    private void SaveData(bool employeBought = false)
    {
        PlayerPrefs.SetFloat(marketOpener.MarketId + StationName + "investedPrice", investedPrice);
        if(employeBought)
            PlayerPrefs.SetInt(marketOpener.MarketId + StationName + "seller", 1);
    }

    void refreshMoney()
    {

        MoneyFiller.fillAmount = investedPrice / TotalMoney;
        MoneyText.text = (TotalMoney - investedPrice).ToString();
        SaveData();
    }

    public float Payment(float givenPrice)
    {
        if (canGiveMoney)
        {
            if (TotalMoney - investedPrice > 0)
            {
                investedPrice += givenPrice;
                refreshMoney();
                if (TotalMoney - investedPrice <= 0)
                    HireEmployee(false);

                return -givenPrice;
            }
            else
            {
                HireEmployee(false);
                return -(TotalMoney - investedPrice);
            }
        }
        else return 0;

    }
    void HireEmployee(bool isLoad)
    {
        GetComponent<BoxCollider>().enabled = false;
        var helper = helpersPrefab[Random.Range(0, helpersPrefab.Count)];
        var x = Instantiate(helper, helperParent);
        x.transform.position = waitingPos.position;
        x.transform.LookAt(lookAtPos);
        x.transform.parent = helperParent;
        canvas.enabled = false;
        sellArea.SetActive(false);
        sellArea.GetComponent<SellArea>().sellActive = true;
        x.GetComponent<SellerController>().markets = markets;
        SaveData(true);

        refreshMoney();
        canGiveMoney = false;
        if (!isLoad) GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.OpenStation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canGiveMoney = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canGiveMoney = true;
        }
    }
}
