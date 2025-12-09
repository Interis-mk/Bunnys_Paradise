using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour, ICarrotGenerator
{
    public virtual float baseAmount { get; set; }
    public virtual float multiplier{ get; set; }
    public virtual int generatorAmount{ get; set; }
    public virtual float repeatRate{get; set; }
    public virtual bool isGenerating{get; set;}
    public virtual float baseCost{get; set;}
    
    public virtual void StartGenerating()
    {
        StartCoroutine(GenerateCoroutine());
    }

    public virtual void OnBuy()
    {
        float cost = baseCost * Mathf.Pow(1.15f, generatorAmount);
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
