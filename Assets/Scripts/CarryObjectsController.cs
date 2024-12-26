using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObjectsController : MonoBehaviour
{
    [HideInInspector] public int boxLevel;
    public List<GameObject> FirstLevelPrefeb;
    public List<GameObject> SecondLevelPrefeb;
    public List<GameObject> ThirtLevelPrefeb;

    [HideInInspector] public CarryObjectType first;
    [HideInInspector] public CarryObjectType second;
    [HideInInspector] public CarryObjectType thirt;
}
