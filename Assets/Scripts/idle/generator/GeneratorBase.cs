using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
[RequireComponent(typeof(DialogueOnHover))]
public abstract class GeneratorBase : MonoBehaviour, ICarrotGenerator
{
    public virtual float baseAmount { get; set; }
    public virtual float multiplier{ get; set; }
    public virtual int generatorAmount{ get; set; }
    public virtual float repeatRate{get; set; }
    public virtual bool isGenerating{get; set;}
    public virtual float baseCost{get; set;}
    protected virtual DialogueOnHover OnHover{get;set;}
    protected virtual float cost { get; set; }
    
    public virtual void Start()
    {
        cost = baseCost;
        OnHover = gameObject.GetComponent<DialogueOnHover>();
        UpdateDialogue();
    }
    protected virtual void UpdateDialogue()
    {
        OnHover.dialogue = $"carrots per second + {baseAmount} " + $"cost : ${MathF.Round(baseCost * Mathf.Pow(1.15f, generatorAmount), 0, MidpointRounding.ToEven)}";
    }
    public virtual void StartGenerating()
    {
        StartCoroutine(GenerateCoroutine());
    }

    public virtual void OnBuy()
    {
        float cost = baseCost * Mathf.Pow(1.15f, generatorAmount);
        cost = MathF.Round(cost, 0, MidpointRounding.ToEven);
        if (cost * (generatorAmount+1) <= CurrencyManager.instance.currency)
        {
            generatorAmount += 1;
            if (!isGenerating)
            {
                StartGenerating();
                isGenerating = true;
            }
        }
    }
    

    public virtual void Generate()
    {
        CurrencyManager.instance.AddCurrency((baseAmount * generatorAmount) * multiplier);
    }
    
    public virtual IEnumerator GenerateCoroutine()
    {
        while (true)
        {
            Generate();
            yield return new WaitForSeconds(repeatRate);
        }
    }
}
