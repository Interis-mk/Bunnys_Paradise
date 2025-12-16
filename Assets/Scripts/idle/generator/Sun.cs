using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : GeneratorBase
{
    public override float baseAmount{get;set;}
    public override float multiplier{get;set;}
    public override int generatorAmount{get;set;}
    public override float baseCost { get; set; }

    [SerializeField] float BaseAmount;
    [SerializeField] float Multiplier;
    [SerializeField] int AmountOwned;
    [SerializeField] float RepeatRate;
    [SerializeField] float basecost;
    
    public override void Start()
    {
        baseAmount = BaseAmount;
        multiplier = Multiplier;
        generatorAmount = AmountOwned;
        repeatRate = RepeatRate;
        baseCost = basecost;
        base.Start();
    }
}
