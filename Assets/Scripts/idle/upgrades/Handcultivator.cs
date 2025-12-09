using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCultivator : UpgradeBase
{
    public override float addedMultiplier{get;set;}
    public override float addedBaseClick{get;set;}
    public override float amountOwned{get;set;}
    public override float baseCost { get; set; }
    
    [SerializeField]private float multiplier;
    [SerializeField]private float baseClick;
    [SerializeField]private float basecost;

    public override void Start()
    {
        addedMultiplier = multiplier;
        addedBaseClick = baseClick;
        baseCost = basecost;
        base.Start();
    }
}
