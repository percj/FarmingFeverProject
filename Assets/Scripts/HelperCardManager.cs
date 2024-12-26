using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCardManager : MonoBehaviour
{
    [SerializeField] HireHelper helperSpawner;
    [SerializeField] HireHelper workerSpawner;
    [SerializeField] Card SpeedCard;
    [SerializeField] Card CollectSpeed;
    [SerializeField] Card Capacity;

    private void Start()
    {
        if (!SpeedCard.isLoaded)
            SpeedCard.LoadData();
        if (!CollectSpeed.isLoaded)
            CollectSpeed.LoadData();
        if (!Capacity.isLoaded)
            Capacity.LoadData();

        CapacityUpgraded();
        CollectSpeedUpgraded();
        SpeedUpgraded();
    }

    public void SpeedUpgraded()
    {
        helperSpawner.setHelperSpeedLevel(SpeedCard.currLevel);
        workerSpawner.setHelperSpeedLevel(SpeedCard.currLevel);
    }
    public void CollectSpeedUpgraded()
    {
        helperSpawner.setHelperCollectSpeedLevel(CollectSpeed.currLevel);
        workerSpawner.setHelperCollectSpeedLevel(CollectSpeed.currLevel);
    }
    public void CapacityUpgraded()
    {
        helperSpawner.setHelperCapacityLevel(Capacity.currLevel);
        workerSpawner.setHelperCapacityLevel(Capacity.currLevel);
    }
}
