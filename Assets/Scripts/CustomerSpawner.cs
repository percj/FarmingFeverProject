using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{

    public List<MarketControll> markets;
    float spawnTimer;
    [Range(0,5)][SerializeField] float spawnElapsed;
    [SerializeField] List<GameObject> customers;
    [SerializeField] Transform startPos;
    [SerializeField] Transform tutorialStartPos;
    [SerializeField] Transform exitPos;
    [SerializeField] bool isSpawnerActive;
    [HideInInspector] public List<CarryObjectType> openedLandType = new List<CarryObjectType>();
    // Start is called before the first frame update
    void Start()
    {
        openedLandType.Add(CarryObjectType.Tomato);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        if (isSpawnerActive && spawnTimer >= spawnElapsed)
        {

            spawnTimer = 0;
            var spawnedCustomer = customers[UnityEngine.Random.Range(0, customers.Count)];
            var x = Instantiate(spawnedCustomer, transform);
            var customerController = x.GetComponent<CustomerController>();
            x.transform.parent = transform;
            x.transform.position = startPos.position;
            customerController.exitPos = exitPos;
            customerController.markets = markets;
            customerController.openedLandType = openedLandType;
        }
        else
            spawnTimer += Time.deltaTime;
    }
    public void tutorialSpawn()
    {
        if (isSpawnerActive)
        {
            var spawnedCustomer = customers[UnityEngine.Random.Range(0, customers.Count)];
            var x = Instantiate(spawnedCustomer, transform);
            var customerController = x.GetComponent<CustomerController>();
            x.transform.parent = transform;
            x.transform.position = tutorialStartPos.position;
            customerController.selectedCarryObjects.Add(CarryObjectType.Tomato);
            customerController.exitPos = exitPos;
            customerController.markets = markets;
        }
    }
}
