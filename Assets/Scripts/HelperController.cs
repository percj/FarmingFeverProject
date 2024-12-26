using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HelperController : MonoBehaviour
{
    [SerializeField] List<float> speedLevelAmount;
    [SerializeField] List<float> collectSpeedLevelAmount;
    [SerializeField] List<int> capacityLevelAmount;
    [SerializeField] NavMeshAgent agent;
    public Transform Collect;
    public Transform WaitPos;
    [SerializeField] carryObjects carryObjects;
    [SerializeField] Animator animator;
    [HideInInspector] public List<SproutController> sproutLoc;
    float collectTimer;
    [Range(0, 5f)][SerializeField] float collectElapsed;
    float refillTimer;
    [Range(0, 5f)][SerializeField] float refillElapsed;
    SproutController selectedStation;

    public int collectSpeedLevel;
    public int speedLevel;
    public int capacityLevel;

    bool collectFull;

    SproutController currSprout = new SproutController() { isStartGethering = false };

    void Start()
    {
        agent.SetDestination(Collect.position);
        setSpeedLevel(speedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedStation == null) findStation();
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

        animator.SetBool("Carry", carryObjects.carryObjets.Count>0);
        
    }

    void findStation()
    {
        if(carryObjects.carryObjets.Count != 0 && collectFull)
        {
            var selectedStations = sproutLoc.Where(x=> !x.isStart && !x.curHelper && x.landController.stationOpener.isOpen).OrderBy(x => Guid.NewGuid()).Take(1).ToList();
            if (selectedStations.Count == 0)
            {
                selectedStation = null;
                agent.SetDestination(WaitPos.position);
            }
            else
            {
                selectedStation = selectedStations[0]; 
                agent.SetDestination(selectedStations[0].transform.position);
                selectedStation.curHelper = true;
            }
        }
        else
        {
            collectFull = false;
            agent.SetDestination(Collect.position);
        }
            
    }

    private void OnTriggerStay(Collider other)  
    {
        if (other.gameObject.tag == "Collect")
        {
            if(carryObjects.carryLimit > carryObjects.carryObjets.Count)
            {
                collectTimer += Time.deltaTime;
                if (collectTimer > collectElapsed)
                {
                    collectTimer = 0;
                    var carryStation = other.GetComponent<CollectStation>();
                    if (carryStation.removeCarryObject())
                        carryObjects.addObject();
                }
            }
            if(carryObjects.carryLimit == carryObjects.carryObjets.Count)
                collectFull = true;
        }

        if (other.gameObject.tag == "Sprout")
        {
            var sproutController = other.GetComponent<SproutController>();
            if (carryObjects.carryObjets.Count > 0 &&  selectedStation == sproutController)
            {
                refillTimer += Time.deltaTime;
                if (refillTimer > refillElapsed)
                {
                    refillTimer = 0;
                    if (!sproutController.isStart)
                    {
                        sproutController.addObject();
                        carryObjects.removeObject();
                    }
                    selectedStation.curHelper = false;
                    selectedStation = null;
                }
            }
           /* else if (sproutController.isFinish && !sproutController.isStartGethering && carryObjects.carryObjets.Count == 0 && canIntract)
            {
                currSprout = sproutController;
                animator.SetTrigger("Gethering");
                carryBox.spawnPos.gameObject.SetActive(false);
                sproutController.isStartGethering = true;
                movment.canMove = false;
            }*/
        }
    }
    internal void setCapacityLevel(int capacityLevel)
    {
        this.capacityLevel = capacityLevel;
        carryObjects.carryLimit = capacityLevelAmount[--capacityLevel];
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
