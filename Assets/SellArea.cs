using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellArea : MonoBehaviour
{
    [HideInInspector] public bool sellActive = false;
    [SerializeField] Image imageComponent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        sellActive = true;
        imageComponent.color = Color.green;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        sellActive = false;
        imageComponent.color = Color.white;
    }
}
