using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertiliser : UpgradeBase
{
    public override float addedMultiplier{get;set;}
    public override float addedBaseClick{get;set;}
    public override int amountOwned{get;set;}
    
    [SerializeField]private float multiplier;
    [SerializeField]private float baseClick;
    
    // Prefab
    [SerializeField]private GameObject generator;

    private void Update()
    {
        addedMultiplier = multiplier;
        addedBaseClick = baseClick;
    }
    
    public override void OnBuy(int cost)
    {
        if (cost * (amountOwned+1) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (amountOwned+1));
            OnBuyIncrement();

            Instantiate(generator, transform.position, transform.rotation);
        }
    }
    
}
