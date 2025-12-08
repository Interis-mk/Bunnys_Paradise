using UnityEngine;

public interface IUpgrade
{
    public float addedMultiplier{get;set;}
    public float addedBaseClick{get;set;}
    public float amountOwned{get;set;}
    
    public Carrot clickObject{get;set;}
    void OnBuyIncrement();
    void OnBuy(int cost);
    
}
public interface IClickable{
    void IsClicked();
}
