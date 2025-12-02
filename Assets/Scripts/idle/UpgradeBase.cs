using UnityEngine;

public abstract class UpgradeBase : MonoBehaviour, IUpgrade
{
    public virtual float addedMultiplier{get;set;}
    public virtual float addedBaseClick{get;set;}
    public virtual float amountOwned{get;set;}
    public Carrot clickObject{get;set;}

    public virtual void Start()
    {
        clickObject = FindAnyObjectByType(typeof(Carrot)) as Carrot;
    }

    public virtual void OnBuyIncrement()
    {
        clickObject.multiplier += addedMultiplier;
        clickObject.baseAmount += addedBaseClick;
        amountOwned += 1;
    }

    public virtual void OnBuy(int cost)
    {
        if (cost*Mathf.Pow(1.15f, amountOwned) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (amountOwned+1));
            OnBuyIncrement();
        }
    }
}
