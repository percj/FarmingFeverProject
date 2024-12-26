using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectStation : MonoBehaviour
{

    float SpawnTimer;
    [SerializeField] float SpawnElapsed;
    [SerializeField] GameObject CarryPrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] int xIndex;
    [SerializeField] int yIndex;
    [Range(0,10)][SerializeField] float ObjectSpacingForX;
    [Range(0,10)][SerializeField] float ObjectSpacingForY;
    [Range(0,5)][SerializeField] float ObjectSpacingHeight;
    [Range(1,100)][SerializeField] int StationLimit;
    [SerializeField] List<GameObject> objects;

    void Update()
    {
        if(StationLimit > objects.Count) SpawnObject();
    }

    private void SpawnObject()
    {
        if (SpawnTimer > SpawnElapsed)
        {
            var x = Instantiate(CarryPrefab, spawnPos);
            var layerIndexforX = (objects.Count % (xIndex));
            var layerIndexforY = (objects.Count / xIndex % (yIndex));
            x.transform.parent = transform;
            x.transform.position += Vector3.right * (layerIndexforX * ObjectSpacingForX);
            x.transform.position +=  Vector3.forward * (layerIndexforY * ObjectSpacingForY);
            x.transform.position += transform.up * (objects.Count / (xIndex*yIndex)) * ObjectSpacingHeight;
            objects.Add(x);
            SpawnTimer = 0;
        }
        else
            SpawnTimer += Time.deltaTime;
    }

    public bool removeCarryObject()
    {
        if(objects.Count == 0) return false;  
            var deleted = objects.Last();
            Destroy(deleted);
            objects.Remove(objects.Last());
        return true;
    }
}
