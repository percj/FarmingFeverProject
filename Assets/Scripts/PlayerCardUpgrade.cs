using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardUpgrade : MonoBehaviour
{
    [SerializeField] PlayerLogic player;
    [SerializeField] Card SpeedCard;
    [SerializeField] Card CollectSpeed;
    [SerializeField] Card Capacity;

    private void Awake()
    {
        CapacityUpgraded();
        CollectSpeedUpgraded();
        SpeedUpgraded();
    }

    public void SpeedUpgraded()
    {
        if (!SpeedCard.isLoaded)
            SpeedCard.LoadData();
        player.setSpeedLevel(SpeedCard.currLevel);
    }
    public void CollectSpeedUpgraded()
    {
        if (!CollectSpeed.isLoaded)
            CollectSpeed.LoadData();
        player.setCollectSpeedLevel(CollectSpeed.currLevel);
    }
    public void CapacityUpgraded()
    {
        if (!Capacity.isLoaded)
            Capacity.LoadData();
        player.setCapacityLevel(Capacity.currLevel);
    }
}
