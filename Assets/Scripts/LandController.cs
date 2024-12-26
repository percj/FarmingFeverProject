using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{
    public List<SproutController> sprouts;
    [HideInInspector] public Dictionary<SproutController,bool> sproutStatus;
    public StationOpener stationOpener;
    void Awake()
    {
        sproutStatus = new Dictionary<SproutController, bool>();
    }
}
