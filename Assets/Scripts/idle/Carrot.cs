using System;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Carrot : MonoBehaviour
{
    float multiplier = 1;
    float baseamount = 1;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IsClicked()
    {
        CurrencyManager.instance.AddCurrency(baseamount * multiplier);
    }
    
}
