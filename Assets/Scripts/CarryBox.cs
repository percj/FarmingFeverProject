using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CarryObjectType
{
    Tomato,
    EggPlant,
    Corn,
    Carrot
}

public class CarryBox : MonoBehaviour
{
    [Range(1, 100)] public int carryLimit;
    [SerializeField] public List<CarryObjectsController> carryBoxs;
    [SerializeField] GameObject carryBoxPrefeb;
    public Transform spawnPos;
    [Range(0, 10)][SerializeField] float objectSpacing;


    public void addObject(CarryObjectType carryObjectType)
    {
        CarryObjectsController carryBoxController=null;
        if (carryBoxs == null || carryBoxs.Count == 0 || carryBoxs.Last().boxLevel == 3)
        {

            var x = Instantiate(carryBoxPrefeb, transform);
            x.transform.parent = spawnPos;
            x.transform.position = spawnPos.position;
            x.transform.position += x.transform.up * objectSpacing * carryBoxs.Count;
            carryBoxController = x.GetComponent<CarryObjectsController>();
            carryBoxs.Add(carryBoxController);
        }
        if (carryBoxController == null) carryBoxController = carryBoxs.Where(x => x.boxLevel != 3).First();
        switch(carryBoxController.boxLevel)
        {
            case 0:
                carryBoxController.FirstLevelPrefeb[carryObjectType.GetHashCode()].SetActive(true);
                carryBoxController.first = carryObjectType;
                carryBoxController.boxLevel += 1;
                break;
            case 1:
                carryBoxController.SecondLevelPrefeb[carryObjectType.GetHashCode()].SetActive(true);
                carryBoxController.second = carryObjectType;
                carryBoxController.boxLevel += 1;
                break;
            case 2:
                carryBoxController.ThirtLevelPrefeb[carryObjectType.GetHashCode()].SetActive(true);
                carryBoxController.thirt = carryObjectType;
                carryBoxController.boxLevel += 1;
                break;
        }
    }
    public CarryObjectType removeObject()
    {
        if (carryBoxs.Count == 0) return CarryObjectType.EggPlant;
        var x = carryBoxs.Last();
        CarryObjectType type = CarryObjectType.EggPlant;
        switch (x.boxLevel)
        {
            case 0:
                type = x.first;
                break;
            case 1:
                type = x.second;
                break;
            case 2:
                type = x.thirt;
                break;
        }
        carryBoxs.Remove(x);
        Destroy(x.gameObject);
        x = null;
        return type;
    }
    public void removeObject(CarryObjectType carryObjectType)
    {
        var selected = carryBoxs.Where(x => (x.first == carryObjectType && x.FirstLevelPrefeb.Any(x=> x.activeInHierarchy == true)) ||
                                         (x.second == carryObjectType && x.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                         (x.thirt == carryObjectType && x.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))).Reverse().FirstOrDefault();
        if (selected == null) return;


        if (selected.thirt == carryObjectType && selected.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))
                selected.ThirtLevelPrefeb.ForEach(x => x.SetActive(false));
        else if (selected.second == carryObjectType && selected.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true))
            selected.SecondLevelPrefeb.ForEach(x => x.SetActive(false));
        else if (selected.first == carryObjectType && selected.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true))
            selected.FirstLevelPrefeb.ForEach(x => x.SetActive(false));

        selected.GetComponent<CarryObjectsController>().boxLevel -= 1;
        if (!selected.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true) &&
            !selected.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true) &&
            !selected.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))
        {
            carryBoxs.Remove(selected);
            Destroy(selected.gameObject);

            var counter = 0;
            foreach (var carryObject in carryBoxs)
            {
                carryObject.transform.position = spawnPos.position;
                carryObject.transform.position += carryObject.transform.up * objectSpacing * counter;
                counter++;
            }

        }
        selected = null;
    }

  

    public bool hasAnySpecificObject(CarryObjectType carryObjectType)
    {
        var sortedList  = carryBoxs.Where(x => (x.first == carryObjectType && x.FirstLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                         (x.second == carryObjectType && x.SecondLevelPrefeb.Any(x => x.activeInHierarchy == true)) ||
                                         (x.thirt == carryObjectType && x.ThirtLevelPrefeb.Any(x => x.activeInHierarchy == true))).FirstOrDefault();
        return sortedList;
    }

}
