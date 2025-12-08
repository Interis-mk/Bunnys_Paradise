using System.Collections;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour
{
    public virtual float baseAmount { get; set; }
    public virtual float multiplier{ get; set; }
    public virtual int generatorAmount{ get; set; }
    public virtual float repeatRate{get; set; }
    public virtual bool isGenerating{get; set;}
    
    public virtual void StartGenerating()
    {
        StartCoroutine(GenerateCoroutine());
    }

    public virtual void OnBuy(int cost)
    {
        if (cost * (generatorAmount+1) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (generatorAmount+1));
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