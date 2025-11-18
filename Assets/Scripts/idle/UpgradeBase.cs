using UnityEngine;

public abstract class UpgradeBase : MonoBehaviour, IUpgrade
{
    public virtual float addedMultiplier{get;set;}
    public virtual float addedBaseClick{get;set;}
    public virtual int amountOwned{get;set;}
    public Carrot clickObject{get;set;}

    public virtual void Start()
    {
        clickObject = FindObjectOfType<Carrot>();
    }

    public virtual void OnBuyIncrement()
    {
        clickObject.multiplier += addedMultiplier;
        clickObject.baseAmount += addedBaseClick;
        amountOwned += 1;
    }

    public virtual void OnBuy(int cost)
    {
        if (cost * (amountOwned+1) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (amountOwned+1));
            OnBuyIncrement();
        }
    }
}
