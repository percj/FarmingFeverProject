using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class CustomerController : MonoBehaviour
{
    [HideInInspector] public MarketControll selectedStation;
    [HideInInspector] public List<MarketControll> markets;
    [HideInInspector] public Transform exitPos;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
   

    bool willExit;
    bool wanderState = false;
    bool inStation = false;
    bool observing = false;

    [Header("=== Frustrade Logic ===")]
    [SerializeField] bool canFrustrurate;
    [SerializeField] GameObject frustrateCanvas;
    [SerializeField] Image frustrate;
    [SerializeField] GameObject frustrateAngry;
    [Range(0,20)][SerializeField] float frustrateElapsed;
    bool isFrustrurate;
    float frustrateTimer;

    [Header("=== Selection ===")]
    [HideInInspector] public int selectedObjectCount;
    [HideInInspector] public List<CarryObjectType> selectedCarryObjects = new List<CarryObjectType>();
    [HideInInspector] public List<CarryObjectType> openedLandType = new List<CarryObjectType>();
    [HideInInspector] public MarketControll observeMarket;
    private int observeSeconds = 5;

    void Start()
    {
        System.Random random = new System.Random();
        if (openedLandType.Count < 4)
            selectedObjectCount = random.Next(openedLandType.Count + 1);
        else
            selectedObjectCount = random.Next(openedLandType.Count);
        for (int i = 0; i < selectedObjectCount;)
        {
            var carrryObject = openedLandType.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (!selectedCarryObjects.Contains(carrryObject))
            {
                selectedCarryObjects.Add(carrryObject);
                i++;
            }
        }

        List<MarketControll> station = new List<MarketControll>();
        for(int i = 0; i < selectedCarryObjects.Count; i++)
        {
            station = markets.Where(x => x.currentCustomers.Count < x.queue.Count && x.gameObject.activeInHierarchy && x.objectType == selectedCarryObjects.First()).OrderBy(x => x.currentCustomers.Count).ThenByDescending(x => x.currObjectAmount).Take(1).ToList();
            if (station.Count > 0)
                break;
            else
                selectedCarryObjects.Remove(selectedCarryObjects[i]);
        }

        if (station.Count > 0)
        {
            selectedStation = station[0];
            selectedStation.currentCustomers.Add(gameObject);

            agent.SetDestination(selectedStation.queue[selectedStation.currentCustomers.Count - 1].transform.position);
        }
        else
        {
            selectedStation = null;
            agent.SetDestination(exitPos.position);
            willExit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControl();

        if (!wanderState && inStation)
        {
            StartCoroutine(WanderState());
        }
        
    }

    IEnumerator ObserveState()
    {
        while (Vector3.Distance(agent.destination, transform.position) > 0.2f)
        {
            yield return null;
        }
        transform.LookAt(observeMarket.transform.position);

        yield return new WaitForSeconds(observeSeconds);

        observing = false;

        List<MarketControll> station = new List<MarketControll>();

        for (int i = 0; i < selectedCarryObjects.Count; i++)
        {
            station = markets.Where(x => x.currentCustomers.Count < x.queue.Count && x.gameObject.activeInHierarchy && x.objectType == selectedCarryObjects.First()).OrderBy(x => x.currentCustomers.Count).ThenByDescending(x => x.currObjectAmount).Take(1).ToList();
            if (station.Count > 0)
                break;
            else
                selectedCarryObjects.Remove(selectedCarryObjects[i]);
        }

        if (station.Count > 0)
        {
            selectedStation = station[0];
            selectedStation.currentCustomers.Add(gameObject);
            inStation = false;
            wanderState = false;

            agent.SetDestination(selectedStation.queue[selectedStation.currentCustomers.Count - 1].transform.position);
        }
        else
        {
            
            agent.SetDestination(exitPos.position);
            willExit = true;
        }


        yield return null;
    }

    IEnumerator WanderState()
    {
        wanderState = true;
        //if (canFrustrurate)
        //{
        //    frustrateCanvas.SetActive(true);

        //    while (frustrateTimer < frustrateElapsed)
        //    {
        //        frustrateTimer += Time.deltaTime;
        //        frustrate.fillAmount = frustrateTimer / frustrateElapsed;
        //        if (selectedStation.objects.Count(x => x.activeSelf) > 0)
        //        {
        //            animator.SetTrigger("Succeed");
        //            selectedStation.RemoveObject();
        //            frustrateCanvas.SetActive(false);
        //            isFrustrurate = false;
        //            yield return new WaitForSeconds(2);
        //            //selectedStation.moneyAreaController.addMoney();
        //            break;
        //        }
        //        yield return null;

        //    }
        //    frustrateAngry.SetActive(true);


        //}
        //else
        {

            while (selectedStation.objects.Count(x => x.activeSelf) <= 0 || !selectedStation.sellArea.sellActive)
            {
                yield return null;
            }
            //animator.SetTrigger("Succeed");
            var carryBox = GetComponent<CarryBox>();
            carryBox.addObject(selectedCarryObjects.First());
            animator.SetBool("Carry", true);
            selectedStation.moneyAreaController.addMoney();

            selectedStation.RemoveObject();
            selectedCarryObjects.Remove(selectedCarryObjects.First());

            yield return new WaitForSeconds(2);

        }

        selectedStation.currentCustomers.Remove(gameObject);
        foreach (var item in selectedStation.currentCustomers)
        {
            item.GetComponent<CustomerController>().agent.SetDestination(selectedStation.queue[selectedStation.currentCustomers.IndexOf(item)].transform.position);
        }

        if (selectedCarryObjects.Any())
        {
            var selectedObservePos = markets.Where(x => x.gameObject.activeInHierarchy).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            observeMarket = selectedObservePos;
            var obserPos = selectedObservePos.observePos.transform.position;
            var newPos = new Vector3(obserPos.x+ UnityEngine.Random.Range(-1,1),obserPos.y, obserPos.z + UnityEngine.Random.Range(-1, 1));
            agent.SetDestination(newPos);


            observing = true;
            StartCoroutine(ObserveState());
        }
        else
        {
            agent.SetDestination(exitPos.position);
            willExit = true;
        }

        yield return null;
    }

    private void AnimationControl()
    {
        if (Vector3.Distance(agent.destination, transform.position) > 0.1f)
        {
            animator.SetBool("run", true);
            agent.isStopped = false;
        }
        else 
        {
            if (selectedStation != null && Vector3.Distance(agent.destination, selectedStation.queue.First().transform.position) < 0.1f && selectedStation.currentCustomers.IndexOf(gameObject) == 0)
                inStation = true;
            animator.SetBool("run", false);
            agent.isStopped = true;
            if (willExit)
            {
                Destroy(gameObject);
                return;
            }

        }
    }
}
