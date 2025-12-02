using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ribbon : UpgradeBase
{
    public override float addedMultiplier{get;set;}
    public override float addedBaseClick{get;set;}
    public override float amountOwned{get;set;}
    
    [SerializeField]private float multiplier;
    [SerializeField]private float baseClick;

    public override void Start()
    {
        base.Start();
        addedMultiplier = multiplier;
        addedBaseClick = baseClick;
    }
}
