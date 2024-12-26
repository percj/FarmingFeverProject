using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


enum HireHelperType {
    Worker,
    Farmer
}

public class HireHelper : MonoBehaviour
{

    [Header("=== Identifier ===")]
    public string ID;
    public string StationName;


    private int helperCap = 5;
    private int workerCap = 5;
    public TextMeshProUGUI countText;
    [SerializeField] GameObject LockedFiller;
    [SerializeField] GameObject FullCap;
    [SerializeField] List<GameObject> helpersPrefab;
    [SerializeField] List<HelperController> helpers;
    [SerializeField] List<WorkerController> workers;
    [SerializeField] Transform helperParent;
    [SerializeField] Transform startPos;
    [SerializeField] Transform waitingPos;
    [SerializeField] Transform CollectPos;
    public List<SproutController> sprouts;
    [SerializeField] HireHelperType currType;
    [SerializeField] Markets markets;

    int SpeedLevel=1;
    int CollectSpeedLevel=1;
    int CapacityLevel=1;

    public float TotalMoney;
    bool canGiveMoney;
    [HideInInspector]public float investedPrice;
    [SerializeField] Image MoneyFiller;
    [SerializeField] TextMeshProUGUI MoneyText;
    void Start()
    {
        LoadData();
        refreshMoney();
    }

    private void LoadData()
    {               
        var helpersCount = PlayerPrefs.GetInt(ID + StationName + "helpersCount", 0);
        var workersCount= PlayerPrefs.GetInt(ID + StationName + "workersCount", 0);
        for (int i = 0; i < helpersCount; i++) HireEmployee(true);
        for (int i = 0; i < workersCount; i++) HireEmployee(true);
        investedPrice = PlayerPrefs.GetFloat(ID + StationName + "investedPrice", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(ID + StationName + "investedPrice", investedPrice);
        PlayerPrefs.SetInt(ID + StationName + "helpersCount", helpers.Count);
        PlayerPrefs.SetInt(ID + StationName + "workersCount", workers.Count);
    }

    void refreshMoney()
    {
        if (currType == HireHelperType.Farmer)
        {
            countText.text = helpers.Count.ToString() + "/" + helperCap.ToString();

            if (helpers.Count == helperCap)
            {
                GetComponent<BoxCollider>().enabled = false;
                LockedFiller.SetActive(false);
                FullCap.SetActive(true);
            }
        }

        else if (currType == HireHelperType.Worker)
        {
            countText.text = workers.Count.ToString() + "/" + workerCap.ToString();

            if (workers.Count == workerCap)
            {
                GetComponent<BoxCollider>().enabled = false;
                LockedFiller.SetActive(false);
                FullCap.SetActive(true);
            }
        }
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
        if(currType == HireHelperType.Farmer)
        {
            var helper = helpersPrefab[Random.Range(0, helpersPrefab.Count)];
            var x = Instantiate(helper, helperParent);
            x.transform.position = startPos.position;
            x.transform.parent = helperParent;
            var helperConroller = x.GetComponent<HelperController>();
            helperConroller.sproutLoc = sprouts;
            helperConroller.Collect = CollectPos;
            helperConroller.WaitPos = waitingPos;
            helperConroller.setSpeedLevel(SpeedLevel);
            helperConroller.setCapacityLevel(CapacityLevel);
            helperConroller.setCollectSpeedLevel(CollectSpeedLevel);
            helpers.Add(helperConroller);
            investedPrice = 0;
            TotalMoney = 1000 * (helpers.Count + 1);
            refreshMoney();
            canGiveMoney = false;
            if (!isLoad) GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.OpenStation);
        }
        else if(currType == HireHelperType.Worker)
        {
            var worker = helpersPrefab[Random.Range(0, helpersPrefab.Count)];
            var x = Instantiate(worker, helperParent);
            x.transform.position = startPos.position;
            x.transform.parent = helperParent;
            var workerConroller = x.GetComponent<WorkerController>();
            workerConroller.sprouts = sprouts;
            workerConroller.markets = markets.Market;
            workerConroller.WaitPos = waitingPos;
            workerConroller.setSpeedLevel(SpeedLevel);
            workerConroller.setCapacityLevel(CapacityLevel);
            workerConroller.setCollectSpeedLevel(CollectSpeedLevel);
            workers.Add(workerConroller);
            investedPrice = 0;
            TotalMoney = 1000 * (workers.Count + 1);
            refreshMoney();
            canGiveMoney = false;
            if (!isLoad) GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.OpenStation);
        }
    }

    public void setHelperSpeedLevel(int currLevel)
    {
        SpeedLevel = currLevel;
        foreach (var helper in helpers)
            helper.setSpeedLevel(SpeedLevel);

        foreach (var helper in workers)
            helper.setSpeedLevel(SpeedLevel);
    }

    internal void setHelperCollectSpeedLevel(int currLevel)
    {
        CollectSpeedLevel = currLevel;
        foreach (var helper in helpers)
            helper.setCollectSpeedLevel(CollectSpeedLevel);

        foreach (var helper in workers)
            helper.setCollectSpeedLevel(CollectSpeedLevel);
    }

    internal void setHelperCapacityLevel(int currLevel)
    {
        CapacityLevel = currLevel;
        foreach (var helper in helpers)
            helper.setCapacityLevel(CapacityLevel);

        foreach (var helper in workers)
            helper.setCapacityLevel(CapacityLevel);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
