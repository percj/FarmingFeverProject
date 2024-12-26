using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SellerController : MonoBehaviour
{
    [HideInInspector]public List<MarketControll> markets;
    [SerializeField] Animator animator;

    void Update()
    {
        AnimationControl();
    }

    private void AnimationControl()
    {
        var hasCustomer = false;

        foreach(var market in markets)
        {
            if (market.currentCustomers.Any())
            {
                hasCustomer = true;
                break;
            }
        }

        animator.SetBool("HasCustomer", hasCustomer);
    }
}
