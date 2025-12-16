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
    protected virtual float cost { get; set; }

    public Carrot clickObject{get;set;}
    protected virtual DialogueOnHover OnHover{get;set;}

    public virtual void Start()
    {
        cost = baseCost;
        clickObject = FindAnyObjectByType(typeof(Carrot)) as Carrot;
        OnHover = gameObject.GetComponent<DialogueOnHover>();
        UpdateDialogue();
    }

    protected virtual void UpdateDialogue()
    {
        OnHover.dialogue = $"ClickPower + {addedBaseClick} " + $"cost : ${MathF.Round(baseCost * Mathf.Pow(1.15f, amountOwned), 0, MidpointRounding.ToEven)}";
    }

    public virtual void OnBuyIncrement()
    {
        clickObject.multiplier += addedMultiplier;
        clickObject.baseAmount += addedBaseClick;
        amountOwned += 1;
        UpdateDialogue();
    }

    public virtual void OnBuy()
    {
        cost = baseCost * Mathf.Pow(1.15f, amountOwned);
        cost = MathF.Round(cost, 0, MidpointRounding.ToEven);
        if (cost <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost);
            OnBuyIncrement();
        }
    }
}
