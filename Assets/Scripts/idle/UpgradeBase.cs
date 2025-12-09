using System;
using UnityEngine;
[RequireComponent(typeof(DialogueOnHover))]
public abstract class UpgradeBase : MonoBehaviour, IUpgrade
{
    public virtual float addedMultiplier{get;set;}
    public virtual float addedBaseClick{get;set;}
    public virtual float amountOwned{get;set;}
    public virtual float currentCost { get; set; }
    public virtual float baseCost { get; set; }
    public Carrot clickObject{get;set;}
    public virtual DialogueOnHover OnHover{get;set;}
    public virtual  string tooltips{get;set;}

    public virtual void Start()
    {
        clickObject = FindAnyObjectByType(typeof(Carrot)) as Carrot;
        OnHover = gameObject.GetComponent<DialogueOnHover>();
    }

    public void Update()
    {
        OnHover.dialogue = $"ClickPower + {addedBaseClick} " + $"cost : ${baseCost}";
    }

    public virtual void OnBuyIncrement()
    {
        clickObject.multiplier += addedMultiplier;
        clickObject.baseAmount += addedBaseClick;
        amountOwned += 1;
    }

    public virtual void OnBuy()
    {
        float cost = baseCost * Mathf.Pow(1.15f, amountOwned);
        if (cost <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(currentCost);
            OnBuyIncrement();
            OnHover.StartDialogue();
        }
    }
}
