using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoneyController : MonoBehaviour
{
    public float speed;
    public Transform target;
    public GameObject CoinPrefab;
   
    public void StartCoinMove(Vector3 _intial,float money)
    {
        Vector3 intialpos = Camera.main.WorldToScreenPoint(_intial);
        GameObject _coin = Instantiate(CoinPrefab, transform);
        _coin.transform.parent = transform;
        _coin.SetActive(true);
        StartCoroutine(MoveCoin (_coin, intialpos, money));
    }
    IEnumerator MoveCoin(GameObject obj, Vector3 startPos,float money)
    {
        float time = 0;
        while (time < 1)
        {
            time += speed * Time.deltaTime;
            obj.transform.position = Vector3.Lerp(startPos, target.position, time);
            yield return null;
        }
        GameSingleton.Instance.SetMoney(money);
        Destroy(obj);
        yield return null;
    }
}




