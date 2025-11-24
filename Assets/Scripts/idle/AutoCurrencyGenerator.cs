using UnityEngine;

public class AutoCurrencyGenerator : MonoBehaviour
{ 
    // Runtime variables
    private float timer = 0;
    
    // Dependencies
    private CurrencyManager currencyManager;
    
    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            currencyManager.AddCurrency(1);
        }
    }
}