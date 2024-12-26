using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour
{
    [SerializeField] List<float> speedLevelAmount;
    [SerializeField] List<float> collectSpeedLevelAmount;
    [SerializeField] List<int> capacityLevelAmount;

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] CarryBox carryBox;
    [HideInInspector] public List<SproutController> sprouts;
    [HideInInspector] public List<MarketControll> markets;
    SproutController currSprout;
    private bool workerStartGathering;
    public Transform WaitPos;
    int carryBoxObjectLimit;
    int currCarryBoxObject;
    MarketControll selectedMarket = null;

    float collectTimer;
    [Range(0, 5f)][SerializeField] float collectElapsed;
    float refillTimer;
    [Range(0, 5f)][SerializeField] float refillElapsed;


    public int collectSpeedLevel;
    public int speedLevel;
    public int capacityLevel;
     bool OnWay;
    void Start()
    {
        carryBoxObjectLimit = 3 * carryBox.carryLimit;
        agent.SetDestination(WaitPos.position);
        setSpeedLevel(speedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        decision();
        AnimationControl();
    }

    private void AnimationControl()
    {
        if (Vector3.Distance(agent.destination, transform.position) > 0.2f)
        {
            animator.SetBool("run", true);
            agent.isStopped = false;
        }
        else
        {
            animator.SetBool("run", false);
            agent.isStopped = true;
        }

        animator.SetBool("Carry", carryBox.carryBoxs.Count > 0);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Sprout")
        {
            var sproutController = other.GetComponent<SproutController>();
         
            if (currCarryBoxObject != carryBoxObjectLimit && sproutController.isFinish && !sproutController.isStartGethering && currSprout == sproutController && !workerStartGathering)
            {
                workerStartGathering = true;
                animator.SetTrigger("Gethering");
                carryBox.spawnPos.gameObject.SetActive(false);
                sproutController.isStartGethering = true;
                agent.isStopped = true;

            }
        }
        if (other.gameObject.tag == "Market")
        {
            var marketController = other.GetComponent<MarketControll>();
            if (marketController.currObjectAmount < marketController.objects.Count && selectedMarket == marketController && carryBox.hasAnySpecificObject(marketController.objectType))
            {
                refillTimer += Time.deltaTime;
                if (refillTimer > refillElapsed)
                {
                    refillTimer = 0;
                    marketController.AddObject();
                    carryBox.removeObject(marketController.objectType);
                    currCarryBoxObject--;
                    var hasObjectType = carryBox.carryBoxs.Where(x => (x.first == marketController.objectType && x.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                (x.second == marketController.objectType && x.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                (x.thirt == marketController.objectType && x.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))).Reverse().FirstOrDefault();

                    if (hasObjectType == null || marketController.objects.Count == marketController.currObjectAmount)selectedMarket = null;
                }
            }
        }
    }
    public void removeSprout()
    {
        currSprout.removeObject();
        agent.isStopped = false;
        carryBox.addObject(currSprout.sproutType);
        currSprout.curHelper = false;
        carryBox.spawnPos.gameObject.SetActive(true);
        currCarryBoxObject++;
        currSprout = null;
        workerStartGathering = false;
    }

    MarketControll findMarket(CarryObjectType carryObjectType)
    {
        
        var hasTomato = carryBox.carryBoxs.Where(x => (x.first == carryObjectType && x.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                   (x.second == carryObjectType && x.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                   (x.thirt == carryObjectType && x.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))).Reverse().FirstOrDefault();
        if (hasTomato != null)
            return markets.Where(x => x.objectType == carryObjectType && x.gameObject.activeInHierarchy && x.currObjectAmount < x.objects.Count).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        return null;
    }
    private void decision()
    {
        if (carryBox.carryBoxs.Count == 0)
            OnWay = false;
        if (currSprout == null && !OnWay)
        {
            currSprout = sprouts.Where(x=> x.isFinish && !x.curHelper).FirstOrDefault();
            OnWay = false;
        }

        if (currSprout == null && carryBox.carryBoxs.Count == 0)
        {
            agent.SetDestination(WaitPos.position);
            OnWay = false;
            return;
        }
        else if (currCarryBoxObject == carryBoxObjectLimit || OnWay == true)
        {
            OnWay = true;
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Tomato);
            if(selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.EggPlant);
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Corn);
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Carrot);
            if (selectedMarket == null)
                agent.SetDestination(WaitPos.position);
            if (selectedMarket != null && selectedMarket.currObjectAmount != selectedMarket.objects.Count)
                agent.SetDestination(selectedMarket.intractPos.position);
            else
                selectedMarket = null;
            return;

        }
        else if(currSprout != null && currCarryBoxObject != carryBoxObjectLimit)
        {
            agent.SetDestination(currSprout.transform.position);
            currSprout.curHelper = true;
            if(!currSprout.isFinish)
            {
                currSprout.curHelper = false;
                currSprout = null;
            }
            return;
        }
        else if (currSprout == null && carryBox.carryBoxs.Count > 0)
        {
            OnWay = true;
            
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Tomato);
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.EggPlant);
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Corn);
            if (selectedMarket == null)
                selectedMarket = findMarket(CarryObjectType.Carrot);
            if (selectedMarket == null)
                agent.SetDestination(WaitPos.position);
            if (selectedMarket != null)
                agent.SetDestination(selectedMarket.intractPos.position);
            return;
        }





    }
    internal void setCapacityLevel(int capacityLevel)
    {
        this.capacityLevel = capacityLevel;
        carryBox.carryLimit = capacityLevelAmount[--capacityLevel];
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
        agent.speed = speedLevelAmount[--speedLevel];
    }
}
