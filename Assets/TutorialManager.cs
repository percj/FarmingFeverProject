using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject player;
    [SerializeField] GameObject collect;
    [SerializeField] GameObject market;
    [SerializeField] GameObject land;
    [SerializeField] GameObject marketItem;
    [SerializeField] Transform money;
    [SerializeField] GameObject[] plantableSprouts;
    public GameObject[] popUps;
    private int popUpIndex;

    void Awake()
    {
        popUpIndex = PlayerPrefs.GetInt("index", popUpIndex);
    }
    void Start()
    {
        if(popUpIndex!=6)
            spawner.SetActive(false);

        StartCoroutine(playTutorial());
    }

    private IEnumerator playTutorial()
    {
        popUpActivate();
        if (popUpIndex == 0)
        {
            float timeElapsed = 0;
            while (popUpIndex == 0)
            {
                collect.GetComponent<Outline>().OutlineWidth = Mathf.Lerp(0, 10, (timeElapsed/2) %1.0f);
                timeElapsed += Time.deltaTime;

                if (player.GetComponent<carryObjects>().carryObjets.Count > 0)
                {
                    popUpIndex++;
                    popUpActivate();
                }
                yield return null;
            }
        }
        if (popUpIndex == 1)
        {
            collect.GetComponent<Outline>().OutlineWidth = 0;
            float timeElapsed = 0;
            while (popUpIndex == 1)
            {
                land.GetComponent<Outline>().OutlineWidth = Mathf.Lerp(0, 10, (timeElapsed / 2) % 1.0f);
                timeElapsed += Time.deltaTime;

                for (int i = 0; i < 9; i++)
                {
                    if (plantableSprouts[i].activeInHierarchy == true && popUpIndex == 1)
                    {
                        popUpIndex++;
                        popUpActivate();
                    }
                    yield return null;
                }
                yield return null;
            }
        }
        if (popUpIndex == 2)
        {
            float timeElapsed = 0;
            while (popUpIndex == 2)
            {
                land.GetComponent<Outline>().OutlineWidth = Mathf.Lerp(0, 10, (timeElapsed / 2) % 1.0f);
                timeElapsed += Time.deltaTime;

                if (player.GetComponent<CarryBox>().carryBoxs.Count > 0)
                {
                    popUpIndex++;
                    popUpActivate();
                }
                yield return null;
            }
        }
        if (popUpIndex == 3)
        {
            land.GetComponent<Outline>().OutlineWidth = 0;
            float timeElapsed = 0;
            while (popUpIndex == 3)
            {
                market.GetComponent<Outline>().OutlineWidth = Mathf.Lerp(0, 10, (timeElapsed / 2) % 1.0f);
                timeElapsed += Time.deltaTime;

                if (marketItem.activeInHierarchy == true)
                {
                    popUpIndex++;
                    popUpActivate();
                }
                yield return null;
            }
        }
        if (popUpIndex == 4)
        {
            spawner.SetActive(true);
            spawner.GetComponent<CustomerSpawner>().tutorialSpawn();
            market.GetComponent<Outline>().OutlineWidth = 0;
            while (popUpIndex == 4)
            {
                if (money.childCount > 0)
                {
                    popUpIndex++;
                    popUpActivate();
                }
                yield return null;
            }
        }
        if (popUpIndex == 5)
        {
            while (popUpIndex == 5)
            {
                if (money.childCount == 0)
                {
                    popUpIndex++;
                    PlayerPrefs.SetInt("index", popUpIndex);
                    popUpActivate();
                }
                yield return null;
            }
        }
        if (popUpIndex == 6)
        {
            Destroy(gameObject);
            yield break;
        }
    }

    private void popUpActivate()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }
    }
}
