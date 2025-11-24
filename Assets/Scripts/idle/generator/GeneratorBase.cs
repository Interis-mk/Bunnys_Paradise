using System.Collections;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour
{
    public virtual float baseAmount { get; set; }
    public virtual float multiplier{ get; set; }
    public virtual int amountOwned{ get; set; }
    public virtual float repeatRate{get; set; }
    
    public virtual void StartGenerating()
    {
        StartCoroutine(GenerateCoroutine());
    }

    public virtual void OnBuy(int cost)
    {
        if (cost * (amountOwned+1) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (amountOwned+1));
            amountOwned += 1;
            StartGenerating();
        }
    }
    

    public virtual void Generate()
    {
        CurrencyManager.instance.AddCurrency((baseAmount * amountOwned) * multiplier);
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