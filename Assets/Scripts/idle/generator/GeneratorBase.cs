using UnityEngine;

public class GeneratorBase : MonoBehaviour
{
    public float baseAmount;
    public float multiplier;

    public int amountOwned;
    
    public void StartGenerating()
    {
        InvokeRepeating(nameof(Generate), 1, 1);
    }
    
    public void OnBuyIncrement()
    {
        amountOwned += 1;
    }

    public void OnBuy(int cost)
    {
        if (cost * (amountOwned+1) <= CurrencyManager.instance.currency)
        {
            CurrencyManager.instance.TakeCurrency(cost *  (amountOwned+1));
            OnBuyIncrement();
        }
    }
    

    public void Generate()
    {
        CurrencyManager.instance.AddCurrency(baseAmount * multiplier);
    }
}