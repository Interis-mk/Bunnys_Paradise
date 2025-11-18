using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovel : UpgradeBase{
    public override float addedMultiplier{get;set;}
    public override float addedBaseClick{get;set;}
    public override int amountOwned{get;set;}
    
    [SerializeField]private float multiplier;
    [SerializeField]private float baseClick;
    [SerializeField]private int amount;

    private void Update()
    {
        addedMultiplier = multiplier;
        addedBaseClick = baseClick;
        amount = amountOwned;
    }
}
