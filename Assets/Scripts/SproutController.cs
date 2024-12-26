using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SproutController : MonoBehaviour
{
    [Header("=== Identifier ===")]
    public string sproutID;


    [HideInInspector] public bool isFinish;
    [HideInInspector] public bool isStart;
    [HideInInspector] public bool isStartGethering;
    [HideInInspector] public bool curHelper;

    public LandController landController;
    [SerializeField] List<GameObject> sproutTypes;
    [SerializeField] GameObject effect;
    [SerializeField] float changeSecond;

    [SerializeField] Image fillerImage;
    [SerializeField] GameObject finishImage;
    public CarryObjectType sproutType;




    float fillAmount;

    void Start()
    {
        foreach (var item in sproutTypes)
            item.SetActive(false);

        landController.sproutStatus.Add(this,false);
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        fillerImage.fillAmount = Mathf.Lerp(fillerImage.fillAmount, fillAmount, Time.deltaTime*2.5f/changeSecond);

    }
    private void LoadData()
    {                                        
        var Sprouts = PlayerPrefs.GetString("Sprouts"+ sproutID + landController.stationOpener.ID+ landController.stationOpener.StationName, "");
        if(Sprouts == "DONE")
        {
            isStartGethering = false;
            isStart = true;
            finishImage.SetActive(true);
            sproutTypes[2].SetActive(true);
            isFinish = true;
        }
       // for (int i = 0; i < Sprouts; i++) addObject();

    }
    public void addObject()
    {
        if(!isStart)
            StartCoroutine(StartGrowing());
    }

    public void removeObject()
    {
        isStart = false;
        isFinish = false;
        isStartGethering = false;
        sproutTypes[2].SetActive(false);
        fillAmount = 0;
        finishImage.SetActive(false);
        fillerImage.gameObject.transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetString("Sprouts" + sproutID + landController.stationOpener.ID + landController.stationOpener.StationName, "");
    }

    IEnumerator StartGrowing()
    {
        isStartGethering = false;
        isStart = true;
        PlayerPrefs.SetString("Sprouts" + sproutID + landController.stationOpener.ID + landController.stationOpener.StationName, "DONE");
        landController.sproutStatus[this] = true;
        fillerImage.gameObject.transform.parent.gameObject.SetActive(true);
        fillAmount = 0.33f;
        effect.transform.localScale = Vector3.one * 3;
        var x =Instantiate(effect, transform);
        sproutTypes[0].SetActive(true);

        yield return new WaitForSeconds(changeSecond);

        fillAmount = 0.66f;
        x = Instantiate(effect, transform);
        sproutTypes[0].SetActive(false);
        sproutTypes[1].SetActive(true);

        yield return new WaitForSeconds(changeSecond);

        fillAmount = 1f;
        yield return new WaitForSeconds(changeSecond);
        
        finishImage.SetActive(true);
        x = Instantiate(effect, transform);
        sproutTypes[1].SetActive(false);
        sproutTypes[2].SetActive(true);
        isFinish = true;
        Destroy(x, 2);
        x = null;
    

        yield return null;
    }
}
