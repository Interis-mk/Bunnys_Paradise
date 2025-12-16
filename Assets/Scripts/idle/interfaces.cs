using UnityEngine;

public interface IUpgrade
{
    public float addedMultiplier{get;set;}
    public float addedBaseClick{get;set;}
    public float amountOwned{get;set;}
    
    public Carrot clickObject{get;set;}
    void OnBuyIncrement();
    void OnBuy();
    
}

public interface ICarrotGenerator
{
    public float baseAmount { get; set; }
    public float multiplier{ get; set; }
    public int generatorAmount{ get; set; }
    public float repeatRate{get; set; }
    public bool isGenerating{get; set;}
    public float baseCost{get; set;}
    void OnBuy();
    void StartGenerating();
}

public interface IClickable{
    void IsClicked();
}
