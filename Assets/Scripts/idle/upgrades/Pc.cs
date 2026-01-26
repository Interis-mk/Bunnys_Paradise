using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : UpgradeBase
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

    private void Update()
    {
        if (amountOwned >= 1)
        {
            SceneLoad.instance.LoadScene("Game Room 1");
        }
    }
}