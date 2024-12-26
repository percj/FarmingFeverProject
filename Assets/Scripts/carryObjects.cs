using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class carryObjects : MonoBehaviour
{
    [Range(1,100)]public int carryLimit;
    [SerializeField] public List<GameObject> carryObjets;
    [SerializeField] GameObject carryPrefeb;
    [SerializeField] Transform spawnPos;
    [Range(0, 10)][SerializeField] float objectSpacing;

    public void addObject()
    {
        var x = Instantiate(carryPrefeb, transform);
        x.transform.parent = spawnPos;
        x.transform.position = spawnPos.position;
        x.transform.position += x.transform.up * objectSpacing * carryObjets.Count;
        carryObjets.Add(x);
    }
    public void removeObject()
    {
        if (carryObjets.Count == 0) return;
        var x = carryObjets.Last();
        carryObjets.Remove(x);
        Destroy(x);
        x = null;
    }

}
