using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    [Header("=== Player Identifier ===")]
    public string ID;
    public string Name;

    [SerializeField] float UnlockStationTimer;
    [SerializeField] carryObjects carryObjects;
    [SerializeField] CarryBox carryBox;
    int carryBoxObjectLimit;
    int currCarryBoxObject;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] JoystickLogic movment;
    public MoneyController moneyMoverForUI;
    bool canIntract;

    [Header("=== Timer Logic ===")]

    [Range(0, 5f)][SerializeField] float collectElapsed;
    [Range(0, 5f)][SerializeField] float purchaseElapsed;
    [Range(0, 5f)][SerializeField] float refillElapsed;
    [Range(0, 5f)][SerializeField] float moneyElapsed;
    float refillTimer;
    float collectTimer;
    float purchaseTimer;
    float moneyTimer;


    [Header("== Upgrade Logic ==")]
    [SerializeField] List<float> speedLevelAmount;
    [SerializeField] List<float> collectSpeedLevelAmount;
    [SerializeField] List<int> capacityLevelAmount;
    [SerializeField] List<int> capacityBoxLevelAmount;
    int collectSpeedLevel=1;
    int speedLevel = 1;
    int capacityLevel = 1;
    private bool playerStartGathering;
    SproutController currSprout = new SproutController() { isStartGethering=false};
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        LoadData();
    }

    void Update()
    {
        AnimationControll(); SaveData();
    }
    private void LoadData()
    {
        var tomato = PlayerPrefs.GetInt(ID + Name + "Tomato",0);
        var carrot = PlayerPrefs.GetInt(ID + Name + "Carrot",0);
        var eggplant = PlayerPrefs.GetInt(ID + Name + "EggPlant",0);
        var corn = PlayerPrefs.GetInt(ID + Name + "Corn",0);
        var carryCount = PlayerPrefs.GetFloat(ID + Name + "carryObjectCount", carryObjects.carryObjets.Count);
        var x = PlayerPrefs.GetFloat(ID + Name + "PosX", 0);
        var y = PlayerPrefs.GetFloat(ID + Name + "PosY", 0);
        var z = PlayerPrefs.GetFloat(ID + Name + "PosZ", 2);
        transform.position = new Vector3(x, y, z);
        for (int i = 0; i < carryCount; i++) 
            carryObjects.addObject();
        for (int i = 0; i < tomato; i++)
            carryBox.addObject(CarryObjectType.Tomato);
        for (int i = 0; i < carrot; i++)
            carryBox.addObject(CarryObjectType.Carrot);
        for (int i = 0; i < eggplant; i++)
            carryBox.addObject(CarryObjectType.EggPlant);
        for (int i = 0; i < corn; i++)
            carryBox.addObject(CarryObjectType.Corn);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(ID + Name + "Tomato", howManySpecificObjectHas(CarryObjectType.Tomato));
        PlayerPrefs.SetInt(ID + Name + "Carrot", howManySpecificObjectHas(CarryObjectType.Carrot));
        PlayerPrefs.SetInt(ID + Name + "EggPlant", howManySpecificObjectHas(CarryObjectType.EggPlant));
        PlayerPrefs.SetInt(ID + Name + "Corn", howManySpecificObjectHas(CarryObjectType.Corn));
        PlayerPrefs.SetFloat(ID + Name + "carryObjectCount",carryObjects.carryObjets.Count);
        PlayerPrefs.SetFloat(ID + Name + "PosX", transform.position.x);
        PlayerPrefs.SetFloat(ID + Name + "PosY", transform.position.y);
        PlayerPrefs.SetFloat(ID + Name + "PosZ", transform.position.z);
    }
   
    private void AnimationControll()
    {
        animator.SetBool("Carry", (carryObjects.carryObjets.Count > 0 || carryBox.carryBoxs.Count > 0) && !currSprout.isStartGethering);
        canIntract = animator.GetFloat("speed")==0;
    }

    private void OnTriggerExit(Collider other)
    {
        GameSingleton.Instance.UI.UpgradeHelper.SetActive(false); 
        GameSingleton.Instance.UI.UpgradePlayer.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Collect" && carryObjects.carryLimit > carryObjects.carryObjets.Count && carryBox.carryBoxs.Count == 0)
        {
            collectTimer += Time.deltaTime;
            if (collectTimer > collectElapsed)
            {
                collectTimer = 0;
                var carryStation = other.GetComponent<CollectStation>();
                if(carryStation.removeCarryObject())
                {
                    carryObjects.addObject();
                    GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.Take);
                }
            }
        }
        if (other.gameObject.tag == "Refill" && (0 < carryObjects.carryObjets.Count || 0 < carryBox.carryBoxs.Count))
        {
            refillTimer += Time.deltaTime;
            if (refillTimer > refillElapsed)
            {
                refillTimer = 0;
                var carryStation = other.GetComponent<RefillArea>();
                if (0 < carryObjects.carryObjets.Count)
                {
                    carryStation.goToTrash(carryObjects.carryObjets.Last());
                    carryObjects.carryObjets.Last().transform.parent = null;
                    carryObjects.carryObjets.Remove(carryObjects.carryObjets.Last());
                }
                else
                {
                    var lastbox = carryBox.carryBoxs.Last();
                    currCarryBoxObject -= lastbox.boxLevel;
                    carryStation.goToTrash(lastbox.gameObject);

                    lastbox.transform.parent = null;
                    carryBox.carryBoxs.Remove(lastbox);
                }
            }
        }

        if (other.gameObject.tag == "Station" && canIntract)
        {
            var intractStation = other.GetComponent<StationController>();
            var money = CalculateGivenMoney(intractStation.unlockedManager.TotalMoney, (intractStation.unlockedManager.TotalMoney - intractStation.unlockedManager.investedPrice));
            if (!intractStation.unlockedManager.isUnlocked && GameSingleton.Instance.Money >= money)
            {
                GameSingleton.Instance.SetMoney(intractStation.unlockedManager.Payment(money));
            }

        }
        if (other.gameObject.tag == "HireHelper" && canIntract)
        {
            var hireHelper = other.GetComponent<HireHelper>();
            var money = CalculateGivenMoney(hireHelper.TotalMoney, (hireHelper.TotalMoney - hireHelper.investedPrice));
            if (GameSingleton.Instance.Money >= money && money != 0)
            {
                GameSingleton.Instance.SetMoney(hireHelper.Payment(money));
            }

        }
        if (other.gameObject.tag == "HireSeller" && canIntract)
        {
            var hireHelper = other.GetComponent<HireSeller>();
            var money = CalculateGivenMoney(hireHelper.TotalMoney, (hireHelper.TotalMoney - hireHelper.investedPrice));
            if (GameSingleton.Instance.Money >= money && money != 0)
            {
                GameSingleton.Instance.SetMoney(hireHelper.Payment(money));
            }

        }
        if (other.gameObject.tag == "StationObjects" && canIntract)
        {
            var intractStation = other.gameObject.transform.parent.GetComponent<StationController>();
            if (intractStation.unlockedManager.isUnlocked && 0 < carryObjects.carryObjets.Count)
            {
                refillTimer += Time.deltaTime;
                if (refillTimer > refillElapsed)
                {
                    refillTimer = 0;
                    intractStation.addObject();
                    carryObjects.removeObject();
                    GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.Drop);
                }
            }
        }
        if (other.gameObject.tag == "MoneyArea")
        {
            var moneyAreaController = other.gameObject.transform.parent.GetComponent<MoneyAreaController>();
            if (moneyAreaController.moneys.Count > 0)
            {
                moneyTimer += Time.deltaTime;
                if (moneyTimer > moneyElapsed)
                {
                    moneyTimer = 0;
                    var addingMoney = moneyAreaController.moneyAmount;
                    moneyMoverForUI.StartCoinMove(moneyAreaController.moneys.Last().transform.position, addingMoney);
                    moneyAreaController.removeMoney();
                }
            }
        }
        if (other.gameObject.tag == "Coin")
        {
            moneyMoverForUI.StartCoinMove(other.transform.position, 1000);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Sprout")
        {
            var sproutController = other.GetComponent<SproutController>();
            if (carryObjects.carryObjets.Count > 0 && !sproutController.isStart)
            {
                refillTimer += Time.deltaTime;
                if (refillTimer > refillElapsed)
                {
                    refillTimer = 0;
                    sproutController.addObject();
                    carryObjects.removeObject();
                    GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.Drop);
                }
            }
            else if(currCarryBoxObject != carryBoxObjectLimit && sproutController.isFinish && !sproutController.isStartGethering && carryObjects.carryObjets.Count == 0 && canIntract && !playerStartGathering)
            {
                playerStartGathering = true;
                currSprout = sproutController;
                animator.SetTrigger("Gethering");
                carryBox.spawnPos.gameObject.SetActive(false);
                sproutController.isStartGethering = true;
                movment.canMove = false;
            }
        }
        if(other.gameObject.tag =="Market")
        {
            var marketController = other.GetComponent<MarketControll>();
            if(marketController.currObjectAmount<marketController.objects.Count && carryBox.hasAnySpecificObject(marketController.objectType))
            {
                refillTimer += Time.deltaTime;
                if (refillTimer > refillElapsed)
                { 
                    refillTimer = 0;
                    marketController.AddObject();
                    carryBox.removeObject(marketController.objectType);
                    currCarryBoxObject--;
                }
            }
        }


        if (other.gameObject.tag == "OpenStation" && canIntract)
        {
            moneyElapsed += Time.deltaTime;
            if (moneyTimer < moneyElapsed)
            {
                moneyElapsed = 0;
                var stationOpener = other.GetComponent<StationOpener>();
                var money = CalculateGivenMoney(stationOpener.TotalMoney, (stationOpener.TotalMoney - stationOpener.investedPrice));
                if (GameSingleton.Instance.Money >= money && money != 0)
                {
                    GameSingleton.Instance.SetMoney(stationOpener.Payment(money));
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "UpgradeHelper") GameSingleton.Instance.UI.UpgradeHelper.SetActive(true);
        if (other.gameObject.tag == "UpgradePlayer") GameSingleton.Instance.UI.UpgradePlayer.SetActive(true);
    }

    public void removeSprout()
    {
        currSprout.removeObject();
        movment.canMove = true;
        carryBox.addObject(currSprout.sproutType);
        carryBox.spawnPos.gameObject.SetActive(true);
        currCarryBoxObject++;
        playerStartGathering = false; 
    }
    internal void setCapacityLevel(int capacityLevel)
    {
        this.capacityLevel = capacityLevel;
        carryObjects.carryLimit = capacityLevelAmount[--capacityLevel];
        carryBox.carryLimit = capacityBoxLevelAmount[capacityLevel];
        carryBoxObjectLimit = carryBox.carryLimit * 3;
    }

    internal void setCollectSpeedLevel(int collectSpeedLevel)
    {
        this.collectSpeedLevel = collectSpeedLevel;
        refillElapsed = collectSpeedLevelAmount[--collectSpeedLevel];
        collectElapsed = collectSpeedLevelAmount[collectSpeedLevel];
    }

    internal void setSpeedLevel(int speedLevel)
    {
        this.speedLevel = speedLevel;
        movment.speed = speedLevelAmount[--speedLevel];
    }
    int CalculateGivenMoney(float totalPrice, float needed)

    {
        float timer = purchaseTimer;
        if (purchaseTimer < 0.02f)
            timer = 0.02f;
        float givePerTime = UnlockStationTimer / timer;
        int willGive = (int)(totalPrice / givePerTime);

        var money = GameSingleton.Instance.Money;

        if (money >= willGive && needed >= willGive)
        {
            purchaseElapsed = 0;
            return willGive;
        }
        else if (money >= 1000 && needed >= 1000)
        {
            purchaseElapsed = 0;
            return 1000;
        }
        else if (money >= 100 && needed >= 100)
        {
            purchaseElapsed = 0;
            return 100;
        }
        else if (money >= 10 && needed >= 10)
        {
            purchaseElapsed = 0;
            return 10;
        }
        else if (money >= 1 && needed >= 1)
        {
            purchaseElapsed = 0;
            return 1;
        }
        return 0;
    }
    public int howManySpecificObjectHas(CarryObjectType carryObjectType)
    {
        var firstCount = carryBox.carryBoxs.Where(x => x.first == carryObjectType && x.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true)).Count();
        var secondCount = carryBox.carryBoxs.Where(x => x.second == carryObjectType && x.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true)).Count();
        var thirtCount = carryBox.carryBoxs.Where(x => x.thirt == carryObjectType && x.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true)).Count();
        return firstCount + secondCount + thirtCount;
    }
}

